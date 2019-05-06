<%@ Page Title="MyAccount"  Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="MyAccount.aspx.cs" Inherits="WebApplication18.WebForm1" %>


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
    <div class="form-group" id="DivStoreEdit" style="visibility:hidden">
  <label for="usr">Store id:</label>
  <input type="text" class="form-control"  id="storeId" name="storeId" >
  <button type="submit" name="btnStoreEdit" id="btnStoreEdit" class="btn btn-default">Submit</button>
</div>
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
               window.location.href = baseUrl+"/Pages/editStore";
              
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



