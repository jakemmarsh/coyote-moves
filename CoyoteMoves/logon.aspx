<%@ Page language="c#" AutoEventWireup="true" %>
<%@ Import Namespace="FormsAuth" %>
<!DOCTYPE html>
<head>
    <meta charset="utf-8" />
    <title>Coyote Moves: Login</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <meta name="viewport" content="width=device-width" />
    <link rel="stylesheet" type="text/css" href="public/css/login.css" media="screen" />
</head>
<html>
  <body>
      <div class="block">
        <div class="block__inner">
            <h1 class="super">COYOTE MOVES</h1>
            <div id="systemAlert" class="island">
                <asp:Label ID="errorLabel" Runat=server></asp:Label>
            </div>
                <div class="block__inner__object">
                    <form id="Login" method="post" runat="server">                    
                        <ul>
                            <li class="cf">
                                <asp:Label ID="Label1" Runat=server class="s"><h2>Username</h2></asp:Label>
                                <asp:TextBox ID="txtUsername" Runat=server class="text-input one-whole" placeholder="john.smith"></asp:TextBox>
                            </li>
                            <li class="cf">
                                <asp:Label ID="Label4" Runat=server class="s"><h2>Password</h2></asp:Label>
                                <asp:TextBox ID="txtPassword" Runat=server TextMode=Password class="text-input one-whole"></asp:TextBox>
                            </li>
                            <li class="cf">
                                <asp:Button ID="btnLogin" Runat=server Text="Sign in" OnClick="Login_Click" CssClass="btn btn--primary fl"></asp:Button>
                            </li>
                        </ul>
                    </form>
                 </div>
            </div>
          </div>
    <script src="Public/js/lib/jquery-1.10.2.js"></script>
    <script src="Public/js/lib/modernizr.js"></script>
    <script src="Public/js/lib/jquery.backstretch.min.js"></script>
    <script>
        function initBackgrounds() {
            // Randomizing and feeding images to Backstretch
            var images = [
                "Public/img/bg-01.jpg",
                "Public/img/bg-02.jpg",
                "Public/img/bg-03.jpg",
                "Public/img/bg-04.jpg",
                "Public/img/bg-05.jpg",
                "Public/img/bg-06.jpg",
                "Public/img/bg-07.jpg",
                "Public/img/bg-08.jpg"],
                randomizeArray = function (arr) {
                    var i = arr.length,
                        j,
                        tempi,
                        tempj;
                    while (--i) {
                        j = Math.floor(Math.random() * (i + 1));
                        tempi = arr[i];
                        tempj = arr[j];
                        arr[i] = tempj;
                        arr[j] = tempi;
                    }
                }
            randomizeArray(images);
            console.log($.backstretch);
            $.backstretch(images, {
                // Speed options of Backstretch
                fade: 1500,
                duration: 30000
            });
        };

        $(function () {
            initBackgrounds();
        });

    </script>
  </body>
</html>
<script runat=server>
protected void Page_Load(object sender, EventArgs e)
{
    txtUsername.Focus();
}
    
void Login_Click(Object sender, EventArgs e)
{
  String adPath = "LDAP://prdgxdc01.coyotelogistics.local"; //Fully-qualified Domain Name
  LdapAuthentication adAuth = new LdapAuthentication(adPath);
  try
  {
    if(true == adAuth.IsAuthenticated(txtUsername.Text, txtPassword.Text))
    {
      String groups = adAuth.GetGroups();

      //Create the ticket, and add the groups.
      bool isCookiePersistent = true;
      FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,  txtUsername.Text,
	DateTime.Now, DateTime.Now.AddMinutes(60), isCookiePersistent, groups);
	
      //Encrypt the ticket.
      String encryptedTicket = FormsAuthentication.Encrypt(authTicket);
		
      //Create a cookie, and then add the encrypted ticket to the cookie as data.
      HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

      if(true == isCookiePersistent)
	authCookie.Expires = authTicket.Expiration;
				
      //Add the cookie to the outgoing cookies collection.
      Response.Cookies.Add(authCookie);		

      //You can redirect now.
      Response.Redirect(FormsAuthentication.GetRedirectUrl(txtUsername.Text, false));
    }
    else
    {
      errorLabel.Text = "Authentication did not succeed. Check user name and password.";
    }
  }
  catch(Exception ex)
  {
    errorLabel.Text = "Error authenticating. " + ex.Message;
  }
}
</script>