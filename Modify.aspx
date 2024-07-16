<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Modify.aspx.cs" Inherits="Route.Modify" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Modify</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/css/bootstrap-datepicker.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />

    <style>
        .toast-custom {
            position: fixed;
            top: 10px;
            right: 20px;
            width: 300px;
            background-color: #fff;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            padding: 15px;
        }

        .toast-success {
            border-left: 5px solid #28a745; /* Light green */
        }

        .toast-danger {
            border-left: 5px solid #dc3545; /* Red */
        }

        .form-control {
            text-align: center;
        }

        @media (max-width: 768px) {
            .col-12 {
                text-align: center;
            }

            #btnOpenModal {
                width: 100%;
            }
        }

        .navbar-white .navbar-toggler {
            border-color: rgba(0, 0, 0, 0.1);
        }

        .navbar-white .navbar-toggler-icon {
            background-image: url("data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(0, 0, 0, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
        }
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js"></script>
    <script>
        function showToast(message, styleClass) {
            var toast = $('<div class="toast-custom ' + styleClass + '">' + message + '</div>').appendTo('#toastContainer');

            // Show the toast
            toast.fadeIn();

            // Move existing toasts down
            $('.toast-custom').not(toast).each(function () {
                $(this).animate({ top: "+=" + (toast.outerHeight() + 10) }, 'fast');
            });

            // Hide the toast after 3 seconds
            setTimeout(function () {
                toast.fadeOut(function () {
                    // Remove the toast from DOM after fadeOut
                    $(this).remove();

                    // Move remaining toasts up
                    $('.toast-custom').each(function (index) {
                        $(this).animate({ top: "-=" + (toast.outerHeight() + 10) }, 'fast');
                    });
                });
            }, 3000);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <nav class="navbar navbar-expand-lg navbar-white bg-white">
            <div class="container">
                <a class="navbar-brand" runat="server" href="~/Home">SYNAPSE</a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ml-auto">
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/Home">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/Create">Create</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/Map">Map</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
        <hr />

        <div class="container body-content">
            <div class="headtag">
                <asp:Label ID="lblUserName" runat="server" Style="color: black; float: right; margin-top: 0px; margin-bottom: -20px; margin-right: 20px"></asp:Label>
            </div>
            <table style="width: 100%; font-family: Calibri; font-size: small">
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lbl_msg" Text="" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Large" BackColor="LightPink"></asp:Label>
                    </td>
                </tr>
            </table>
            <h2 style="text-align: center; margin-top: 20px;">Modify Route</h2>
            <br />

            <div class="container">
                <div class="row">
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:TextBox ID="DistCode" runat="server" CssClass="form-control" placeholder="Dist Code" ReadOnly="true"></asp:TextBox>
                        <%--<asp:DropDownList ID="DistDrp" runat="server" AutoPostBack="true" class="form-control">
                            <asp:ListItem Text="DistCode" Value=""></asp:ListItem>
                        </asp:DropDownList>--%>
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:TextBox ID="RtCode" runat="server" CssClass="form-control" placeholder="Route Code" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:TextBox ID="RtName" runat="server" CssClass="form-control" placeholder="Route Name"></asp:TextBox>
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="MnfId" runat="server" AutoPostBack="true" class="form-control">
                            <asp:ListItem Text="MNF Code" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="RtType" runat="server" AutoPostBack="true" class="form-control">
                            <asp:ListItem Text="Route Type" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="RtCoverage" runat="server" AutoPostBack="true" class="form-control">
                            <asp:ListItem Text="Route Coverage" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <button type="button" class="form-control" id="btnOpenModal" data-toggle="modal" data-target="#exampleModalCenter">
                            Call Days
                        </button>
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" class="form-control">
                            <asp:ListItem Text="Status" Value=""></asp:ListItem>
                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                            <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:Button ID="Submit" runat="server" Text="Modify" CssClass="btn btn-success form-control" OnClick="btnModify_Click" />
                    </div>
                </div>

                <%--                <div class="row mt-3">
                    <div class="col-12">
                        <div class="grid-wrapper">
                            <asp:GridView ID="RouteGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                                Style="margin-bottom: 0px; text-align: center;">
                                <Columns>
                                    <asp:BoundField DataField="DistCode" HeaderText="DistCode" />
                                    <asp:BoundField DataField="RouteCode" HeaderText="RouteCode" />
                                    <asp:BoundField DataField="RouteName" HeaderText="RouteName" />
                                    <asp:BoundField DataField="MnfCde" HeaderText="MNFCode" />
                                    <asp:BoundField DataField="RouteTypeName" HeaderText="RouteType" />
                                    <asp:BoundField DataField="CoverageName" HeaderText="RouteCoverage" />
                                    <asp:BoundField DataField="WeekDay" HeaderText="Call Days" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />

                                    <asp:TemplateField HeaderText="Modify">
                                        <ItemTemplate>
                                            <asp:Button ID="EditBtn" runat="server" Text="Edit" CssClass="btn btn-primary form-control" OnClick="EditBtn_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>--%>
            </div>

            <%-- Modal for Call Days --%>
            <div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLongTitle">Call Days</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body" style="max-height: 400px; overflow-y: auto;">
                            <div class="form-group">
                                <asp:GridView ID="DayId" runat="server" AutoPostBack="True" CssClass="table table-bordered form-group"
                                    AutoGenerateColumns="false" DataKeyNames="" Style="margin-bottom: -18px; text-align: center">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <div style="margin-right: 10px;">
                                                    <input type="checkbox" id="selectAllCheckBox" runat="server" style="margin-left: -3px;" class="form-check-input" onclick="selectAllCheckboxes(this)" />
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="margin-right: 10px;">
                                                    <input type="checkbox" id="CheckBox1" runat="server" class="form-check-input rowCheckbox" style="margin-left: -3px;" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Call Days">
                                            <ItemTemplate>
                                                <asp:Label ID="DayLabel" runat="server" Text='<%# Eval("DayName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="header-hidden" />
                                    <RowStyle CssClass="fixed-height-row" BackColor="#FFFFFF" />
                                </asp:GridView>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button type="button" class="btn btn-primary" onclick="selectItems()" data-dismiss="modal">Select</button>
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Notification Label --%>
            <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
            <asp:HiddenField ID="hdnBusinessType" runat="server" />
            <asp:HiddenField ID="hdnRole" runat="server" />
        </div>
    </form>

    <%-- Script for selectall checkboxes in Modal --%>
    <script type="text/javascript">
        function selectAllCheckboxes(source) {
            var checkboxes = document.querySelectorAll('.rowCheckbox');
            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].checked = source.checked;
            }
        }
    </script>

</body>
</html>
