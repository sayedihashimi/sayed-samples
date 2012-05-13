<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ParameterizeConStringConfig.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sample</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>This sample shows how to parameterize connection strings in files other than web.config</h2>

        <h3>See ParameterizeConStringConfig.wpp.targets for the details</h3>


        <h3>Connection strings:</h3>
        <ul>
            <% foreach (ConnectionStringSettings cn in ConfigurationManager.ConnectionStrings)
               { %>
                <li><%:cn.Name %>: <em><%:cn.ConnectionString %></em></li>
            <%} %>
        </ul>
    </div>
    </form>
</body>
</html>
