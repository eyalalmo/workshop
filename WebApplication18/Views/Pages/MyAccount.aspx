<%@ Page Title="MyAccount"  Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="MyAccount.aspx.cs"  Inherits="WebApplication18.Views.Pages.MyAccount" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>
 <div class="container">
<div class="dropdown">
    <button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown">Options
    </button>
    <ul class="dropdown-menu">
    <li><a href="#" id="AddStore">Add Store</a></li>
    <li><a href="#" id="editStore">Edit Stores </a></li>
  </ul>
   </div>
     </div>
   
    <script type="text/javascript">
        $(document).ready(function () {
            var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
            $("#AddStore").click(function () {
               event.preventDefault();
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
               window.location.href = baseUrl+"/AddStore";
            });
            $("#editStore").click(function () {
                event.preventDefault();
               window.location.href = baseUrl+"/EditStore";
              
            });

           

        });

    </script>
 </asp:Content>



