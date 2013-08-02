<%@ Page language="c#" AutoEventWireup="true" %>
<%@ Import Namespace="FormsAuth" %>
<!DOCTYPE html>
<head>
    <meta charset="utf-8" />
    <title>Coyote Moves: Login</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <meta name="viewport" content="width=device-width" />
    <link rel="stylesheet" type="text/css" href="public/css/style.css" media="screen" />
</head>
<html>
  <body>	
    <form id="Login" method="post" runat="server" class="full-width full-height">
        <div id="form-inner">
            <h1>Coyote Moves</h1>
            <div class="tr">
                <div class="td">
                    <asp:Label ID="Label2" Runat=server >Username:</asp:Label>
                </div>
                <div class="td">
                    <asp:TextBox ID=txtUsername Runat=server placeholder="Windows Username"></asp:TextBox>
                </div>
            </div>
            <div class="tr">
                <div class="td">
                    <asp:Label ID="Label3" Runat=server >Password:</asp:Label>
                </div>
                <div class="td">
                    <asp:TextBox ID="txtPassword" Runat=server TextMode=Password placeholder="Windows Password"></asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnLogin" Runat=server Text="Login" OnClick="Login_Click"></asp:Button>
            <asp:Label ID="errorLabel" Runat=server ForeColor=#ff3300></asp:Label>
        </div>
    </form>	
  </body>
</html>
<script runat=server>
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