<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Basket.aspx.cs" Inherits="WebApplication18.Views.Pages.Basket" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>

    <!--
    <table class="table">
  <thead>
    <tr>
      <th scope="col">#</th>
      <th scope="col">First</th>
      <th scope="col">Last</th>
      <th scope="col">Handle</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <th scope="row">1</th>
      <td>Mark</td>
      <td>Otto</td>
      <td>@mdo</td>
    </tr>
    <tr>
      <th scope="row">2</th>
      <td>Jacob</td>
      <td>Thornton</td>
      <td>@fat</td>
    </tr>
    <tr>
      <th scope="row">3</th>
      <td>Larry</td>
      <td>the Bird</td>
      <td>@twitter</td>
    </tr>
  </tbody>
</table>
        -->
    <script src="https://use.fontawesome.com/c560c025cf.js"></script>
    <div class="container">
          <div class="card shopping-cart">
            <div class="card-header bg-dark text-light">
                <i class="fa fa-shopping-cart" aria-hidden="true"></i>
                Shopping Cart
                <div class="clearfix"></div><a></a>
            </div>
            <div id="AllProductsInBasket" class ="col-md-12">
                
            </div>
            <div id="checkout" class ="col-md-12">
            </div>   
            <div id="totalPrice"></div>
        </div>
    </div>
        

      <script type="text/javascript">

        $(document).ready(function () {
            
               var mainDiv = document.getElementById('AllProductsInBasket');
               var checkoutDiv = document.getElementById('checkout');
               
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
               console.log(baseUrl);

               
                console.log("before jquery");
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl + "/api/user/getShoppingBasket",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                        function (response) {
                            console.log("response");
                            if (response != "") {
                                console.log(response);
                           
                                var str ="<div class=\"card-body\"><div class=\"row\style=\"background-color:black\"> </div></div>";
                                /*    "<table class =\"table table-bordered text-center\">"
                                + "<thead>"
                                + "<tr>"
                                + "<td style = \"width:80px\" > Image</td>"
                                + "<td>Name </td>"
                                + "<td>Price</td>"
                                + "<td style=\"width:110px\">ID</td>"
                                + "<td style=\"width:110px\">Quantity</td>"
                                + "<td style=\"width:60px\">Delete</td>"
                                + "</tr>"
                                + "</thead>"
                                + "<tbody>";*/
                            var products = response.split(";");
                            for (i = 0; i < products.length-1; i++){
                                
                                    
                                var productfields = products[i].split(",");
                                var productName = productfields[0];
                                var price = productfields[1];
                                var id = productfields[2];
                                var quantity = +productfields[3];
                                str +=
                                    "<hr><div class=\"card-body\">" +
                                    "<div class=\"row\"> <div class=\"col-12 col-sm-12 col-md-2 text-center\"><img src=\"../Images/NoImageAvailabe.jpg\"" + "height=\"80\" /></div><div class=\"col-12 text-sm-center col-sm-12 text-md-left col-md-6\"><h4 class=\"product-name\"><strong>"+ productName +"</strong></h4></div> <div class=\"col-12 col-sm-12 text-sm-center col-md-4 text-md-right row\"> <div class=\"col-2 col-sm-2 col-md-4 text-md-right\" style=\"padding-top: 5px\">  <h5><strong>$"+ price + "<span class=\"text-muted\"> x</span></strong></h5></div><div class=\"quantity\"><input type=\"button\" value=\"+\" onclick=\"plusQuantity("+id+","+quantity+");\" class=\"plus\"><input type=\"text\" value=\""+quantity+"\" title=\"Qty\" class=\"qty\"><input type=\"button\" value=\"-\" onclick=\"minusQuantity("+id+","+quantity+");\" class=\"minus\"></div><div class=\"col-2 col-sm-2 col-md-2 text-right\"><button type=\"button\" class=\"btn btn-danger\" onclick=\"deleteRow("+id+");\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></button></div></div></div></div>";
                            }
                            //str += " </tbody>" + "</table>";
                                totalPrice();
                            mainDiv.innerHTML = str;
                            checkoutDiv.innerHTML = "<div class=\"pull-right\" style=\"margin: 10px\"><input type=\"button\" class=\"btn btn-success\" value=\"Checkout\" onclick=\" checkout()\"></div>";
//<a href=\"#\" class=\"btn btn-success\">Checkout</a>

                                
                    

                        }
                        else {
                              mainDiv.innerHTML = "<div align=\"center\"> <img src=\"../Images/emptyCart2.png\"" + "height=\"400\" /></div>";
                            //<form><input type=\"image\" src=\"../Images/trash.png\" name=\"Delete\" width=\"25\" height=\"25\" align=\"top\" alt=\"Stop sign\ onclick=\"deleteRow(this);\"></form>
                            //<img src=\"../Images/trash.png\"" + "height=\"27\" />
                            //                                    "<td ><img src=\"../Images/NoImageAvailabe.jpg\"" + "height=\"60\" /></td><td style=\"vertical-align :middle\">" + productName + "</td><td style=\"vertical-align:middle\">" + price + "</td><td style=\"vertical-align:middle\">" + id + "</td><td style=\"vertical-align:middle\">" + quantity + "</td><td style=\"vertical-align :middle\"><form><input type=\"image\" src=\"../Images/trash.png\" name=\"Delete\" width=\"25\" height=\"25\" align=\"top\" alt=\"Stop sign\ onclick=\"deleteRow();\"></form></td></tr>";

                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          });

          function plusQuantity(id, quantity) {
              event.preventDefault();
              console.log(id);
              var plusQuantity = quantity + 1;
              var getUrl = window.location;
              var baseUrl = getUrl.protocol + "//" + getUrl.host
               jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/user/setProductQuantity?product=" + id+"&quantity="+plusQuantity,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {

                            location.reload();
                        }
                        else {
                            console.log(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          }
          
          function minusQuantity(id, quantity) {
              event.preventDefault();
              console.log(id);
              var minusQuantity = quantity - 1;
              var getUrl = window.location;
              var baseUrl = getUrl.protocol + "//" + getUrl.host
               jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/user/setProductQuantity?product=" + id+"&quantity="+minusQuantity,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {

                            location.reload();
                        }
                        else {
                            console.log(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          }

           function deleteRow(id) {
              event.preventDefault();
              console.log(id);
              var getUrl = window.location;
              var baseUrl = getUrl.protocol + "//" + getUrl.host
               jQuery.ajax({
                   type: "GET",
                    url: baseUrl+"/api/user/removeProductFromCart?productId=" + id,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {

                            location.reload();
                        }
                        else {
                            console.log(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          }

                function checkout() {
                                
                              event.preventDefault();
                              var getUrl = window.location;
                              var baseUrl = getUrl.protocol + "//" + getUrl.host
                              window.location.href = baseUrl+"/Checkout/";
                          }
         /* $(".deleteRow").click(function() {
              console.log("11111");
            });*/
          function totalPrice() {
                  var getUrl = window.location;
                  var baseUrl = getUrl.protocol + "//" + getUrl.host
                  var mainDiv2 = document.getElementById('totalPrice');
                  jQuery.ajax({
                      type: "GET",
                      url: baseUrl + "/api/user/basketTotalPrice",
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      success: function (response) {
                          if (response != "") {                   
                           var str = "<div class=\"pull-right\" style=\"margin: 15px\">Total price: <b>$" + response + "</b></div> ";
                            mainDiv2.innerHTML = str;
                             
                          }
                          else {
                              console.log(response);
                          }
                      },
                      error: function (response) {
                          console.log(response);
                      }
                  });
           }
    </script>
</asp:Content>