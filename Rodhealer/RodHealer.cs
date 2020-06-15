using System;
using Terraria;
using TerrariaApi.Server;
using System.Reflection;
using System.Collections.Generic;
using TShockAPI;

namespace RODhealer
{
    [ApiVersion(2, 1)]
    public class Rodhealer : TerrariaPlugin
    {
        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override string Author
        {
            get { return "Ancientgods"; }
        }

        public override string Name
        {
            get { return "Rod healer"; }
        }

        public override string Description
        {
            get { return "Heals you when using the rod of discord!"; }
        }

        public override void Initialize()
        {
            ServerApi.Hooks.NetGetData.Register(this, OnGetData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ServerApi.Hooks.NetGetData.Deregister(this, OnGetData);

            base.Dispose(disposing);
        }

        public Rodhealer(Main game)
            : base(game)
        {
            Order = 1;
        }

        private void OnGetData(GetDataEventArgs e)
        {
            if (e.Handled)
                return;

            if (e.MsgID == PacketTypes.Teleport)
            {
                TSPlayer player = TShock.Players[e.Msg.whoAmI];
                List<int> buffs = new List<int>(player.TPlayer.buffType);
                bool PvP = player.TPlayer.hostile;

                if ((player != null) && (player.Group.HasPermission("rod.healer")) && (player.SelectedItem.netID == 1326) && (buffs.Contains(88)) && !PvP)
                {
                    player.Heal(player.TPlayer.statLifeMax2 / 7);
                }
            }
        }
    }
}
