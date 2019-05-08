<%@ Page Title="MyAccount"  Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="MyAccount.aspx.cs"  Inherits="WebApplication18.Views.Pages.MyAccount" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>
<div class="dropdown">
    <button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown">Options
    <span class="caret"></span></button>
    <ul class="dropdown-menu">
    <li><a href="#" id="AddStore" >Add Store</a></li>
    <li><a href="#" id="editStore"  >edit Store </a></li>
  </ul>
    <div class="form-group" id="DivStoreEdit" style="visibility:hidden">
  <label for="usr">Store id:</label>
  <input type="text" class="form-control"  id="storeId" name="storeId" >
  <button type="submit" name="btnStoreEdit" id="btnStoreEdit" class="btn btn-default">Submit</button>
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
            
             $("#AddProuduct").click(function () {
               event.preventDefault();
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
               window.location.href = baseUrl+"/login";
            });
             $("#ShowProuduct").click(function () {
               event.preventDefault();
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
               window.location.href = baseUrl+"/login";
            });


        });

    </script>
 </asp:Content>



