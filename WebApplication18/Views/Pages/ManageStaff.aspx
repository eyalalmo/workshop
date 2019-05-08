<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageStaff.aspx.cs" Inherits="WebApplication18.Views.Pages.ManageStaff" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron d-flex align-items-center">
  <div class="container">
    <h1><%: Title +"Store Staff Panel" %></h1>   
    <h1> </h1>
      <h1> </h1>
      <div class="container ">
          <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
    <button  name="addManager" id="addManager" class="btn btn-primary" data-toggle="modal" data-target="#myModal">Add Manager</button>
                  </div>
          <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
              <h3>Add an Owner</h3>
              <input class="form-control align-middle" type="text"  visible="false" placeholder="Username"  id="owner" name="owner"/>
        <button type="button" class="btn btn-primary" onClick="confirmOwner()" id="confirm" name="confirm"/>
             Confirm
              </div>
          </div>
       </div>
    
    <div class="container">
	<div id="allRoles" class="row">
        </div>
		</div>
         
  </div>
 
<div class="modal" id="myModal">
  <div class="modal-dialog">
    <div class="modal-content">

      <!-- Modal Header -->
      <div class="modal-header">
        <h2 class="modal-title">Please Enter Username</h2>
        <button type="button" class="close" data-dismiss="modal">&times;</button>
      </div>

      <!-- Modal body -->
      <div class="modal-body">
        <input class="form-control" type="text" placeholder="Manager Username" aria-label="Search" id="username" name="username">
      </div>

      <!-- Modal footer -->
      <div class="modal-footer">
         <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
        <button  name="addM" id="addM" class="btn btn-primary">Add Manager</button>
      </div>

    </div>
  </div>
</div>
    <script type="text/javascript">
        function getRoles(response) {
            var doc = document.getElementById('allRoles');
            doc.innerHTML = "";
            var i;
            var jsonList = JSON.parse(response);
            var role = "Store Owner"
            var HTML;
            for (i = 0; i < jsonList.length; i++) {
                if (!jsonList[i].isOwner)
                    role="Store Manager"
                HTML += `<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
		                   <div class="my-list">
			<span>`+ role + `</span>
            <span> Username:`+ jsonList[i].user.username + `</span>
			<span > Appointed By:`+ jsonList[i].appointedBy.username + `</span>
		    <button type="button" class="btn btn-primary" onClick="removeRole(` + jsonList[i].user.username + `)"/>
             Remove
            </button>
		</div>
		</div>`
                //`+jsonList[i].ProductID+`


            }
            doc.innerHTML = HTML;
        };
        function getName(ID) {

        }
    </script>
    <script type="text/javascript">
        function confirmOwner() {
             var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host
            var username = $("#owner").val();
            var storeId =<%=ViewData["storeId"]%>;
                
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl + "/api/store/addOwner?username=" + username +"&storeID=" + storeId,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response !== "ok") {
                            alert(response);
                        } else
                            alert("Owner successfuly added")
                        
                    },
                    error: function (response) {
                        console.log(response);
                    }
            });
            
        }
        

        
    </script>
<script type="text/javascript">

    $(document).ready(function () {
            
                event.preventDefault();
                  var storeId =<%=ViewData["storeId"]%>;
                var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl + "/api/products/getAllRoles?storeId="+ storeId,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        console.log(response);
                        getRoles(response);
                        
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });

           
     });

    </script>
    </asp:Content>