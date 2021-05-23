using ChatDesktop.Business;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilitiesChat;
using UtilitiesChat.Models.WS;

namespace ChatDesktop
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            SessionStart();
        }

        private void iniciarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionStart();
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Business.Session.oUser = null;
            iniciarSesiónToolStripMenuItem.Enabled = true;
            cerrarSesiónToolStripMenuItem.Enabled = false;
            splitContainer1.Enabled = false;
        }

        #region HELPER
        private void GetDataInit()
        {
            List<ListRoomsResponce> lst = new List<ListRoomsResponce>();

            SecurityRequest oSecurityRequest = new SecurityRequest();
            oSecurityRequest.AccessToken = Business.Session.oUser.AccessToken;

            RequestUtil oRequestUtil = new RequestUtil();
            UtilitiesChat.Models.WS.Reply oReply = oRequestUtil.Execute<SecurityRequest>(Constants.Url.ROOMS, "post", oSecurityRequest);

            lst = JsonConvert.DeserializeObject<List<ListRoomsResponce>>(
                JsonConvert.SerializeObject(oReply.data)
                );
            cboRooms.DataSource = lst;
            cboRooms.DisplayMember = "Name";
            cboRooms.ValueMember = "Id";
        }
        private void SessionStart()
        {
            FrmLogin frmLogin = new FrmLogin();
            frmLogin.ShowDialog();
            if (Business.Session.oUser != null)
            {
                iniciarSesiónToolStripMenuItem.Enabled = false;
                cerrarSesiónToolStripMenuItem.Enabled = true;
                splitContainer1.Enabled = true;
                GetDataInit();
            }

        }
        #endregion
    }
}
