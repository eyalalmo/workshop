<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShoppingCart.aspx.cs" Inherits="WebApplication18.Views.Pages.ShoppingCart" %>

<!DOCTYPE html>

<link href="//maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">
<script src="//maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
<script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
<!------ Include the above in your HEAD tag ---------->

<script src="https://use.fontawesome.com/c560c025cf.js"></script>
<div class="container">
   <div class="card shopping-cart">
            <div class="card-header bg-dark text-light">
                <i class="fa fa-shopping-cart" aria-hidden="true"></i>
                Shipping cart
                <a href="" class="btn btn-outline-info btn-sm pull-right">Continiu shopping</a>
                <div class="clearfix"></div>
            </div>
            <div class="card-body">
                    <!-- PRODUCT -->
                    <div class="row">
                        <div class="col-12 col-sm-12 col-md-2 text-center">
                                <img class="img-responsive" src="http://placehold.it/120x80" alt="prewiew" width="120" height="80">
                        </div>
                        <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                            <h4 class="product-name"><strong>Product Name</strong></h4>
                            <h4>
                                <small>Product description</small>
                            </h4>
                        </div>
                        <div class="col-12 col-sm-12 text-sm-center col-md-4 text-md-right row">
                            <div class="col-3 col-sm-3 col-md-6 text-md-right" style="padding-top: 5px">
                                <h6><strong>25.00 <span class="text-muted">x</span></strong></h6>
                            </div>
                            <div class="col-4 col-sm-4 col-md-4">
                                <div class="quantity">
                                    <input type="button" value="+" class="plus">
                                    <input type="number" step="1" max="99" min="1" value="1" title="Qty" class="qty"
                                           >
                                    <input type="button" value="-" class="minus">
                                </div>
                            </div>
                            <div class="col-2 col-sm-2 col-md-2 text-right">
                                <button type="button" class="btn btn-outline-danger btn-xs">
                                    <i class="fa fa-trash" aria-hidden="true"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                    <hr>
                    <!-- END PRODUCT -->
                    <!-- PRODUCT -->
                    <div class="row">
                        <div class="col-12 col-sm-12 col-md-2 text-center">
                                <img class="img-responsive" src="http://placehold.it/120x80" alt="prewiew" width="120" height="80">
                        </div>
                        <div class="col-12 text-sm-center col-sm-12 text-md-left col-md-6">
                            <h4 class="product-name"><strong>Product Name</strong></h4>
                            <h4>
                                <small>Product description</small>
                            </h4>
                        </div>
                        <div class="col-12 col-sm-12 text-sm-center col-md-4 text-md-right row">
                            <div class="col-3 col-sm-3 col-md-6 text-md-right" style="padding-top: 5px">
                                <h6><strong>25.00 <span class="text-muted">x</span></strong></h6>
                            </div>
                            <div class="col-4 col-sm-4 col-md-4">
                                <div class="quantity">
                                    <input type="button" value="+" class="plus">
                                    <input type="number" step="1" max="99" min="1" value="1" title="Qty" class="qty"
                                           size="4">
                                    <input type="button" value="-" class="minus">
                                </div>
                            </div>
                            <div class="col-2 col-sm-2 col-md-2 text-right">
                                <button type="button" class="btn btn-outline-danger btn-xs">
                                    <i class="fa fa-trash" aria-hidden="true"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                    <hr>
                    <!-- END PRODUCT -->
                <div class="pull-right">
                    <a  class="btn btn-outline-secondary pull-right">
                        Update shopping cart
                    </a>
                </div>
            </div>
            <div class="card-footer">
                <div class="coupon col-md-5 col-sm-5 no-padding-left pull-left">
                    <div class="row">
                        <div class="col-6">
                            <input type="text" class="form-control" placeholder="cupone code">
                        </div>
                        <div class="col-6">
                            <input type="submit" class="btn btn-default" value="Use cupone">
                        </div>
                    </div>
                </div>
                <div class="pull-right" style="margin: 10px">
                    <a class="btn btn-success pull-right">Checkout</a>
                    <div class="pull-right" style="margin: 5px">
                        Total price: <b>50.00€</b>
                    </div>
                </div>
            </div>
        </div>
</div>
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Basket.aspx.cs" Inherits="WebApplication18.Views.Pages.Basket" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
                <a  class="btn btn-outline-info btn-sm pull-right">Continue Shopping</a>
                <div class="clearfix"></div>
            </div>
            
            <div id="AllProductsInBasket" class ="col-md-12">
                
            </div>
            <div id="checkout" class ="col-md-12"> 
            </div>      
        </div>
    </div>
        

      <script type="text/javascript">

        $(document).ready(function () {
            
               var mainDiv = document.getElementById('AllProductsInBasket');
               var checkoutDiv = document.getElementById('checkout');
               
               var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
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
                           
                            var str = "<table class =\"table table-bordered text-center\">"
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
                                + "<tbody>";
                            var products = response.split(";");
                            for (i = 0; i < products.length-1; i++){
                                
                                    
                                var productfields = products[i].split(",");
                                var productName = productfields[0];
                                var price = productfields[1];
                                var id = productfields[2];
                                var quantity = +productfields[3];
                                    str += "<tr>" +
                                    "<td ><img src=\"../Images/NoImageAvailabe.jpg\"" + "height=\"60\" /></td><td style=\"vertical-align :middle\">" + productName + "</td><td style=\"vertical-align:middle\">" + price + "</td><td style=\"vertical-align:middle\">" + id + "</td><td><div class=\"quantity\"><input type=\"button\" value=\"+\" class=\"plus\"><input type=\"number\" step=\"1\" max=\"99\" min=\"1\" value=\"1\" title=\"Qty\" class=\"qty\"><input type=\"button\" value=\"-\" class=\"minus\"></div></td><td><button type=\"button\" class=\"btn btn-danger\"onclick=\"deleteRow("+id+");\" ><i class=\"fa fa-trash\" aria-hidden=\"true\"></i> </button></td></tr>";
                            }
                            str += " </tbody>" + "</table>";
                            mainDiv.innerHTML = str;
                            checkoutDiv.innerHTML = "<div class=\"pull-right\"><input type=\"button\" class=\"btn btn-success\" value=\"Checkout\" onclick=\" checkout()\"></div>";
//<a href=\"#\" class=\"btn btn-success\">Checkout</a>

                        }
                        else {
                              mainDiv.innerHTML = " <h3 style =\"margin-bottom: 30px\">Basket is EMPTY</h3>";
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

          function deleteRow(id) {
              event.preventDefault();
              console.log(id);
              var getUrl = window.location;
              var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
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
    </script>
</asp:Content>

<td style=\"vertical-align :middle\"><form><input type = \"button\" class=\"btn btn-danger\" value = \"Delete\" onclick=\"deleteRow("+id+");\"></form>
     <div class=\"col-2 col-sm-2 col-md-2 text-right\"><button type=\"button\" class=\"btn btn-danger\"onclick=\"deleteRow("+id+");\ ><i class=\"fa fa-trash\" aria-hidden=\"true\"></i> </button></div>
    */
