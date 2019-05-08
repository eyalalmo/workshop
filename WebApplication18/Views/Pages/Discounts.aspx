<%@ Page Title="Discounts" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Discounts.aspx.cs" Inherits="WebApplication18.Views.Pages.Discounts" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>

 <div class="form-group">
  <label for="usr">Add Visible Discount:</label>
  <input type="text" class="form-control" id="percentage" name="percentage">
</div>
    <button type="submit" name="btnVisibleDiscount" id="btnVisibleDiscount" class="btn btn-default">Submit</button>
  <div class="form-group">
    <label for="pwd">Password:</label>
    <input type="password" class="form-control" id="pass" name="pass">
  </div>
  <div class="checkbox">
    <label><input type="checkbox"> Remember me</label>
  </div>
  <button type="submit" name="btnLogin" id="btnLogin" class="btn btn-default">Submit</button>

   <script type="text/javascript">

        $(document).ready(function () {
            console.log("storeID:"+storeID);
            console.log("productID:"+productID);
        
        });

    </script>

</asp:Content>

