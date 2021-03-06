﻿<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="WebApplication18.Views.Pages.Checkout" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>

 <div class="form-group">
  <label for="usr">Address:</label>
  <input type="text" class="form-control" id="address" name="address">
</div>
    <div class="form-group">
  <label for="usr">Name:</label>
  <input type="text" class="form-control" id="name" name="address">
</div>
    <div class="form-group">
  <label for="usr">City:</label>
  <input type="text" class="form-control" id="city" name="address">
</div>
    <div class="form-group">
  <label for="usr">Country:</label>
  <input type="text" class="form-control" id="country" name="address">
</div>
    <div class="form-group">
  <label for="usr">Zip:</label>
  <input type="text" class="form-control" id="zip" name="address">
</div>
  <div class="form-group">
    <label for="creditcard">Credit Card:</label>
    <input type="text" class="form-control" id="creditcard" name="creditcard">
  </div>
<div class="form-group">
    <label for="creditcard">month:</label>
    <input type="text" class="form-control" id="month" name="creditcard">
  </div>
    <div class="form-group">
    <label for="creditcard">year:</label>
    <input type="text" class="form-control" id="year" name="creditcard">
  </div>
    <div class="form-group">
    <label for="creditcard">holder:</label>
    <input type="text" class="form-control" id="holder" name="creditcard">
  </div>
    <div class="form-group">
    <label for="creditcard">CVV:</label>
    <input type="text" class="form-control" id="cvv" name="creditcard">
  </div>
    <div class="form-group">
    <label for="creditcard">Id:</label>
    <input type="text" class="form-control" id="id" name="creditcard">
  </div>

  <div id="totalPrice" class ="form-group"></div>
  <button type="submit" name="btnPurchase" id="btnPurchase" class="btn btn-success">Purchase</button>

   <script type="text/javascript">

       $(document).ready(function () {

           var mainDiv = document.getElementById('totalPrice');
           var getUrl = window.location;
           var baseUrl = getUrl.protocol + "//" + getUrl.host
                  jQuery.ajax({
                      type: "GET",

                    url: baseUrl+"/api/user/basketTotalPrice",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                      success: function (response) {
                          if (response != "") {    
                             var total = response.split(",");

                            var str = "<label for=\"usr\">Total Price: $"+total[0]+"</label>";
                            mainDiv.innerHTML = str;
                        }
                        else {
                            console.log(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
           });
           
            $("#btnPurchase").click(function () {
                event.preventDefault();
                
                address = $("#address").val();
                creditcard = $("#creditcard").val();
                month = $("#month").val();
                year = $("#year").val();
                holder = $("#holder").val();
                cvv = $("#cvv").val();
                id = $("#id").val();
                console.log("dd")
                jQuery.ajax({
                      type: "GET",
                    url: baseUrl+"/api/user/Checkout?address=" + address + "&creditcard=" + creditcard + "&month=" + month + "&year=" + year + "&holder=" + holder + "&cvv=" + cvv,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                      success: function (response) {
                         if (response === "OK") {
                             alert("payment succeded")
                             window.location.href = baseUrl;
                         }
                         else {
                             alert("failed checkout & payment")
                         }
                     }
               });
           });
            });

    </script>

</asp:Content>

