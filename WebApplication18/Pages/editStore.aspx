<%@ Page  Title="Edit Store" Language="C#" AutoEventWireup="true" CodeBehind="editStore.aspx.cs" Inherits="WebApplication18.editStore" %>



<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>
<div class="dropdown">
  <button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown">options
  <span class="caret"></span></button>
  <ul class="dropdown-menu">
    <li><a href="#" id="AddStore" name="AddStore">Add Store</a></li>
    <li><a href="#" id="editStore" name="editStore" >edit Store </a></li>
    <li><a href="#" id="AddProuduct" name="AddProuduct" >Add Prouduct</a></li>
      <li><a href="#" id="ShowProuduct" name="ShowProuduct">Show Prouduct</a></li>
  </ul>
</div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#AddStore").click(function () {
               event.preventDefault();
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
               window.location.href = baseUrl+"/Pages/login";
            });
            $("#editStore").click(function () {
               event.preventDefault();
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
               window.location.href = baseUrl+"/Pages/login";
            });
             $("#AddProuduct").click(function () {
               event.preventDefault();
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
               window.location.href = baseUrl+"/Pages/login";
            });
             $("#ShowProuduct").click(function () {
               event.preventDefault();
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
               window.location.href = baseUrl+"/Pages/login";
            });


        });

    </script>
 </asp:Content>



