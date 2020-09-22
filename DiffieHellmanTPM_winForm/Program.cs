using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiffieHellmanTPM_winForm {
    static class Program {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            ProtocolOption protocolOption = ProtocolOption.Default;
            foreach (string arg in args) {
                if (arg == "-graph") {
                    protocolOption = ProtocolOption.Graph;
                }
                else if (arg == "-eva") {
                    protocolOption = ProtocolOption.Eva;
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormDiffieHellmanTPM(protocolOption));
        }
    }
}
