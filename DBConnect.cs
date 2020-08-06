//Add MySQL Library
using MySql.Data.MySqlClient;
using System.Windows.Forms;

//connection: will be used to open a connection to the database.
//server: indicates where our server is hosted, in our case, it's localhost.
//database: is the name of the database we will use, in our case it's the database we already created earlier which is connectcsharptomysql.
//uid: is our MySQL username.
//password: is our MySQL password.
//connectionString: contains the connection string to connect to the database, and will be assigned to the connection variable.

namespace UIDesign
{
    class DBConnect
    {
        public MySqlConnection connection;
        public string server;
        public string database;
        public string uid;
        public string password;
        public string connectionString;

        //constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        public void Initialize()
        {
            server = "localhost";
            database = "si_engine_test";
            uid = "username";
            password = "password";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server. Start XAMPP or contact admin");
                        break;
                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
                return false;
            }
        }

        //Close Connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
