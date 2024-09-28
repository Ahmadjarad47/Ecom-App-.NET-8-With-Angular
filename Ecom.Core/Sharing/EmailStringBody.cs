using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Sharing
{
    public class EmailStringBody
    {
        public static string send(string email, string token, string component, string message)
        {
            string encodeToken = Uri.EscapeDataString(token);
            return $@"
                     <html> 
                            <head>
<style>
.button{{
 border: none;
      border-radius: 10px;
      padding: 15px 30px;
      color: #fff;
      display: inline-block;
      background: linear-gradient(45deg, #ff7e5f, #feb47b);
      cursor: pointer;
      text-decoration: none;
      box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
      transition: all 0.3s ease;
      font-size: 16px;
      font-weight: bold;
      font-family: 'Arial', sans-serif;
      animation: glow 1.5s infinite alternate;

}}
</style>
</head>
                            <body>
                                <h1>{message}</h1>
                                        <hr>
                                     <br>
                                    <a class=""button"" href=""http://localhost:4200/Account/{component}?email={email}&code={encodeToken}"">
                                            {message}
                                        </a>
                            </body>
                        </html>
                    
                    ";
        }
    }
}
