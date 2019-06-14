<%@ Page Title="StoreDiscount" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StoreDiscount.aspx.cs" Inherits="WebApplication18.Views.Pages.StoreDiscount" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>

 <div class="form-group">
  <h4>Add Visible Discount:</h4>
  <label for="per">Percentage:</label>
  <input type="number"  min="0" max="1" class="form-control" id="percentage" name="percentage">
  <label for="per">Duration:</label>
  <input type="text" class="form-control" id="duration" name="duration">
</div>
    <button type="submit" name="btnVisibleDiscount" id="btnVisibleDiscount" class="btn btn-default">Submit</button>
 <div class="form-group">
  <h4>Add Reliant Discount - Total Price:</h4>
  <label for="per">Percentage:</label>
  <input type="number"  min="0" max="1" class="form-control" id="percentage2" name="percentage2">
  <label for="per">Duration:</label>
  <input type="text" class="form-control" id="duration2" name="duration2">
  <label for="per">Total price:</label>
  <input type="text" class="form-control" id="price" name="price">
</div>
    <button type="submit" name="btnRelianTotalAmount" id="btnRelianTotalAmount" class="btn btn-default">Submit</button>
<div class="form-group">
  <h4>Add Reliant Discount - Minimal amount of same product:</h4>
  <label for="per">Percentage:</label>
  <input type="number"  min="0" max="1" class="form-control" id="percentage3" name="percentage3">
  <label for="per">Duration:</label>
  <input type="text" class="form-control" id="duration3" name="duration3">
    <label for="per">Total amount:</label>
  <input type="number" class="form-control" id="numOfProducts" name="numOfProducts">
   <label for="per">Product ID:</label>
  <input type="number" class="form-control" id="productID3" name="productID3">
  
</div>
     <button type="submit" name="btnRelianSameProduct" id="btnRelianSameProduct" class="btn btn-default">Submit</button>
   <script type="text/javascript">

       $(document).ready(function () {
           var getUrl = window.location;
           var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18";
           var storeID =<%=ViewData["storeID"]%>;
            $("#btnVisibleDiscount").click(function () {
                event.preventDefault();
                percentage = $("#percentage").val();
                duration = $("#duration").val();    
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/store/addVisibleDiscount?storeID=" + storeID + "&percentage=" + percentage+"&duration=" + duration,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "") {
                           alert("Visible Discount added Successfully")   
                           baseUrl + "/StoreDiscount?storeId=" + storeID
                           window.location.href =  baseUrl + "/StoreDiscounts?storeId=" + storeID
                                             
                        }
                        else {
                            alert(response);
                            location.reload();
                        }
                    },
                    error: function (response) {
                        alert(response);
                        location.reload();
                    }
                });
           });

           $("#btnRelianTotalAmount").click(function () {
               event.preventDefault();
               percentage = $("#percentage2").val();
               duration = $("#duration2").val(); 
               price = $("#price").val(); 
               console.log("price: " + price);
               jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/store/addReliantDiscountTotalAmount?storeID=" + storeID + "&totalAmount="+price + "&percentage=" + percentage+"&duration=" + duration,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "") {
                            event.preventDefault();
                            alert("Reliant Discount added Successfully")  
                            baseUrl + "/StoreDiscount?storeId=" + storeID
                            window.location.href =  baseUrl + "/StoreDiscounts?storeId=" + storeID
                                               
                        }
                        else {
                            alert(response);
                            location.reload();
                        }
                    },
                    error: function (response) {
                        console.log("fail");
                        alert(response);
                    }
                });
           });

           $("#btnRelianSameProduct").click(function () {
               event.preventDefault();
               percentage = $("#percentage3").val();
               duration = $("#duration3").val(); 
               numOfProducts = $("#numOfProducts").val();
               productID = $("#productID3").val(); 
               jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/store/addReliantDiscountSameProduct?storeID=" + storeID +"&percentage=" + percentage+"&duration=" + duration+"&numOfProducts=" + numOfProducts+"&productID=" + productID,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response == "") {
                            alert("Reliant Discount added Successfully");
                            baseUrl + "/StoreDiscount?storeId=" + storeID
                            window.location.href =  baseUrl + "/StoreDiscounts?storeId=" + storeID
                        }
                        else {
                            alert(response);
                            location.reload();
                        }
                    },
                    error: function (response) {
                        console.log("fail");
                        alert(response);
                    }
                });
           });

        
        });

    </script>

</asp:Content>

