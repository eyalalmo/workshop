<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="WebApplication18.Views.Pages.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Contact Page</h3>
    <address>
       Hamarganit<br />
        Israel, 8<br />
        <abbr title="Phone">P: 054054054</abbr>
       
    </address>

    <address>
        <strong>Support:</strong>   <a href="mailto:Support@nanabanana.com">Support@nanabanana.com</a><br />
        <strong>Marketing:</strong> <a href="mailto:Marketing@nanabanana.com">Marketing@nanabanana.com</a>
    </address>
</asp:Content>
