<%@ Page Title="Add Product" Language="C#"  AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AddProduct.aspx.cs" Inherits="WebApplication18.Views.Pages.AddProduct" %>

<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
   <h2><%: Title +" For Store Id:  " + ViewData["storeId"] %>  </h2>
   
    
  <div class="form-group">
  
  <label for="usr">Product Name:</label>
  <input type="text" class="form-control" id="productName" name="name">
</div>
    <div class="form-group">
  <label for="usr">Product Category:</label>
  <input type="text" class="form-control" id="productCategory" name="name">
</div>
    <div class="form-group">
  <label for="usr">Price:</label>
  <input type="number" class="form-control" id="price" name="name">
</div>
    <div class="form-group">
  <label for="usr">Rank:</label>
  <input type="number" class="form-control" id="rank" name="name">
</div>
    <div class="form-group">
  <label for="usr">Quantity:</label>
  <input type="number" class="form-control" id="quantityLeft" name="name">
</div>
 

  
 
  <button type="submit" class="btn btn-primary" name="btnAdd" id="btnAdd" >Add Product</button>

   <script type="text/javascript">

        $(document).ready(function () {
            
            $("#btnAdd").click(function () {
                event.preventDefault();
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
              var storeID  =<%=ViewData["storeId"]%>;
                productName = $("#productName").val();
                productCategory = $("#productCategory").val();
                price = $("#price").val();
                rank = $("#rank").val();
                quantityLeft = $("#quantityLeft").val();
                
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/store/addProduct?productName=" + productName + "&productCategory=" + productCategory + "&price=" + price+ "&rank=" + rank+ "&quantityLeft=" + quantityLeft+ "&storeID=" + storeID,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {
                             alert("Product added successfuly")                     
                        }
                        else {
                            alert(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            });
        });

    </script>

</asp:Content>
