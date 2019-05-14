using System.Collections.Generic;
using System.Linq;

namespace Brain
{
    internal static class ConnectionHelper
    {
        public static void RealignAllConnections()
        {
            var contracts = GameScene.Instance.AllChildrenOfType<ContractEntity>();
            var connections = GameScene.Instance.AllChildrenOfType<ConnectionLine>();

            var contractConnections = contracts.SelectMany(x => GetConnected(connections, x)).ToList();
            foreach (var connection in contractConnections)
            {
                EnforceContractDirection(connection);
                var remainingConnections = connections.Except(contractConnections).ToList();
                CascadeSetEnd(remainingConnections, connection.Start);
            }

            var contractChains = contracts.Select(GetConnectionChain).ToList();
            var looseConnections = connections.Except(contractChains.SelectMany(x => x));
            foreach (var connection in looseConnections)
            {
                connection.Kill();
            }

            foreach (var chain in contractChains)
            {
                for (var depth = 0; depth < chain.Count(); depth++)
                {
                    var connection = chain[depth];
                    connection.Wake();
                    var start = connection.Start as HostEntity;
                    start.ChainDepth = depth;
                }
            }
        }

        private static void EnforceContractDirection(ConnectionLine connection)
        {
            if (connection.End is ContractEntity)
            {
                return;
            }

            var tmp = connection.Start;
            connection.Start = connection.End;
            connection.End = tmp;
        }

        private static void CascadeSetEnd(List<ConnectionLine> allConnections, Entity entity)
        {
            var connected = GetConnected(allConnections, entity);
            connected.ForEach(x =>
            {
                allConnections.Remove(x);
                SwapEndsIfNeeded(x, entity);
                x.Wake();
                CascadeSetEnd(allConnections, x.Start);
            });
        }

        private static List<ConnectionLine> GetConnected(List<ConnectionLine> connections, Entity entity)
        {
            return connections
                .Where(x => x.Has(entity))
                .ToList();
        }

        private static void SwapEndsIfNeeded(ConnectionLine connection, Entity entity)
        {
            if (connection.End != entity)
            {
                var tmp = connection.Start;
                connection.Start = connection.End;
                connection.End = tmp;
            }
        }

        public static void KillConnections(HostEntity hostEntity)
        {
            GameScene.Instance.Visit<ConnectionLine>(x =>
            {
                if (x.Has(hostEntity))
                {
                    x.Kill();
                }
            });
        }

        public static void RemoveConnections(Entity entity)
        {
            GameScene.Instance
                .AllChildrenOfType<ConnectionLine>()
                .ForEach(x => {
                    if (x.Has(entity))
                        x.Parent.Remove(x);
                });
            RealignAllConnections();
        }

        private static List<ConnectionLine> GetConnectionChain(Entity x)
        {
            var chain = new List<ConnectionLine>();
            GetConnectionChain(chain, x);
            return chain;
        }

        private static void GetConnectionChain(List<ConnectionLine> chain, Entity entity)
        {
            var connected = GameScene.Instance.AllChildrenOfType<ConnectionLine>().Where(x => x.End == entity);
            foreach (var connection in connected)
            {
                chain.Add(connection);
                GetConnectionChain(chain, connection.Start);
            }
        }
    }
}