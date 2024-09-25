<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Transfer.aspx.cs" Inherits="Route.Transfer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transfer</title>
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

            .grid-wrapper {
                overflow-x: auto;
                -webkit-overflow-scrolling: touch;
            }

                .grid-wrapper table {
                    width: 100%;
                    display: block;
                }

                    .grid-wrapper table thead,
                    .grid-wrapper table tbody {
                        display: table;
                        width: 100%;
                    }
        }

        .navbar-white .navbar-toggler {
            border-color: rgba(0, 0, 0, 0.1);
        }

        .navbar-white .navbar-toggler-icon {
            background-image: url("data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(0, 0, 0, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
        }

        .grid-wrapper {
            max-height: 300px;
            max-width: 100%;
            overflow: auto;
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

        function showNoteAlert() {
            $('#noteAlert').show();
        }

        function hideNoteAlert() {
            $('#noteAlert').hide();
        }

        function showNoteAlert2() {
            $('#noteAlert2').show();
        }

        function hideNoteAlert2() {
            $('#noteAlert2').hide();
        }
    </script>
</head>
<body>
    <form id="form2" runat="server">

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
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/Transfer">Transfer</a>
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
            <h2 style="text-align: center; margin-top: 20px;">Transfer Route</h2>
            <br />

            <div class="container">
                <div class="row justify-content-center">
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:Button ID="RouteTransferBtn" runat="server" Text="Route Transfer" CssClass="btn btn-outline-info form-control" OnClick="RouteTransferBtn_Click" />
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:Button ID="RetTransferBtn" runat="server" Text="Retailer Transfer" CssClass="btn btn-outline-info form-control" OnClick="RetTransferBtn_Click" />
                    </div>
                </div>
            </div>

            <div id="routeTransferDiv" runat="server" class="container mt-3" visible="false">
                <div class="row">
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="FromDistDrp" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="FromDistDrp_SelectedIndexChanged">
                            <asp:ListItem Text="From Distributor" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="ToDistDrp" runat="server" AutoPostBack="true" class="form-control">
                            <asp:ListItem Text="To Distributor" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="TypeDrp" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="TypeDrp_SelectedIndexChanged">
                            <asp:ListItem Text="Transfer Type" Value=""></asp:ListItem>
                            <asp:ListItem Text="Existing" Value="Existing"></asp:ListItem>
                            <asp:ListItem Text="Split" Value="Split"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:Button ID="RouteTransferSubmit" runat="server" Text="Transfer" CssClass="btn btn-success form-control" OnClick="RouteTransferSubmit_Click" />
                    </div>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-12">
                    <div class="grid-wrapper">
                        <asp:GridView ID="RouteTransExistGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
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

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <div class="grid-wrapper">
                        <asp:GridView ID="RouteTransSplitGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
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
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div id="retailerTransferDiv" runat="server" class="container" visible="false">
                <div class="row">
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" class="form-control">
                            <asp:ListItem Text="From Dist." Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="true" class="form-control">
                            <asp:ListItem Text="To Dist." Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="true" class="form-control">
                            <asp:ListItem Text="Transfer Type" Value=""></asp:ListItem>
                            <asp:ListItem Text="Existing" Value="Existing"></asp:ListItem>
                            <asp:ListItem Text="Split" Value="Split"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:Button ID="RetTransferSubmit" runat="server" Text="Transfer" CssClass="btn btn-success form-control" />
                    </div>
                </div>

            </div>
        </div>

        <%-- Notification Label --%>
        <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
        <asp:HiddenField ID="hdnBusinessType" runat="server" />
        <asp:HiddenField ID="hdnRole" runat="server" />
    </form>

    <%-- Script for selectall checkboxes in RouteTransferSplitGridview --%>
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
