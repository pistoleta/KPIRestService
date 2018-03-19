using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Trazabilidad.Objetos
{
    public class Usuario
    {
        private UInt16 id = 0;
        private string nombre = null;
        private string correo = null;
        private string password = null;
        private int rol = 0;
        public UInt16 Id { get { return id; } set { id = value; } }
        public string Nombre { get { return nombre; } set { nombre = value; } }
        public string Correo { get { return correo; } set { correo = value; } }
        public string Password { get { return password; } set { password = value; } }
        public int Rol { get { return rol; } set { rol = value; } }
    }
}
