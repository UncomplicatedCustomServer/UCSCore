using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Linq;
using ServerRound = Exiled.API.Features.Round;

namespace UncomplicatedCustomServerCore.API.Features.Round.Messages
{
    internal class RoundUpdateMessage : MessageBase
    {
        public override RoundActionType Action => RoundActionType.RoundUpdate;

        public double RoundTime => ServerRound.ElapsedTime.TotalSeconds;

        public long RoundStartTime => ((DateTimeOffset)ServerRound.StartedTime).ToUnixTimeSeconds();

        public int RoundUptimes => ServerRound.UptimeRounds;

        public bool IsStarted => ServerRound.IsStarted;

        public bool InProgress => ServerRound.InProgress;

        public bool IsLobby => ServerRound.IsLobby;

        public bool IsLobbyLocked => ServerRound.IsLobbyLocked;

        public int Kills => ServerRound.Kills;

        public int KillsByScp => ServerRound.KillsByScp;

        public string[] AliveSides => ServerRound.AliveSides.Select(side => side.ToString()).ToArray();

        public int EscapedDClasses => ServerRound.EscapedDClasses;

        public int EscapedScientists => ServerRound.EscapedScientists;

        public int SurvivingScps => ServerRound.SurvivingSCPs;

        public int ChangedIntoZombies => ServerRound.ChangedIntoZombies;

        public int LobbyWaitingTime => ServerRound.LobbyWaitingTime;

        public string NextRoundAction => ServerRound.NextRoundAction.ToString();

        public int FoundationForcesAlive => Player.List.Count(p => p.Role.Team is Team.FoundationForces);

        public int ScpsAlive => Player.List.Count(p => p.Role.Team is Team.SCPs);

        public int ChaosInsurgencyAlive => Player.List.Count(p => p.Role.Team is Team.ChaosInsurgency);

        public int ScientistsAlive => Player.List.Count(p => p.Role.Team is Team.Scientists);

        public int ClassDAlive => Player.List.Count(p => p.Role.Type is RoleTypeId.ClassD);

        public double Tps => Server.Tps;

        public double MaxTps => Server.MaxTps;

        public static RoundUpdateMessage Create() => new();
    }
}
