<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllProducts.aspx.cs" Inherits="WebApplication18.Pages.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <button type="submit" name="showAll" id="showAll" class="btn btn-default">Show All Products</button>

<script type="text/javascript">

        $(document).ready(function () {
            
            $("#showAll").click(function () {
                event.preventDefault();
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host

                jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/products/getAllProducts",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var HTML = ""
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            });
        });

    </script>
    </asp:Content>