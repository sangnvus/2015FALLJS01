"use strict";
var service = angular.module("DDLService", []);
var directive = angular.module("DDLDirective", []);
var app = angular.module("AdminApp", ["ngRoute", "ngAnimate", "ngSanitize", "DDLService",
    "DDLDirective", 'angular-loading-bar', 'textAngular', 'toastr', 'ui.bootstrap', 'monospaced.elastic',
    'datatables', 'datatables.bootstrap', 'oitozero.ngSweetAlert', 'blockUI']);

// Show Routing.
app.config(["$routeProvider", function ($routeProvider) {
    $routeProvider.when("/dashboard",
        {
            redirectTo: "/"
        });
    $routeProvider.when("/",
        {
            templateUrl: "/AdminPartial/Dashboard",
            controller: 'AdminDashBoardController',
            activeTab: 'dashboard',
            breadcrumb: ['Quản lý chung', 'Thống kê'],
            title: 'Thống kê',
            resolve: {
            }
        });

    $routeProvider.when("/category",
        {
            templateUrl: "/AdminPartial/Category",
            controller: 'AdminCategoryController',
            activeTab: 'category',
            breadcrumb: ['Quản lý danh mục', 'Danh sách danh mục'],
            title: 'Quản lý danh mục',
            resolve: {
                listcategory: ['$rootScope', '$q', 'AdminCategoryService', 'CommmonService', function ($rootScope, $q, AdminCategoryService, CommmonService) {
                    var promise = AdminCategoryService.getCategories();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });

    $routeProvider.otherwise({
        redirectTo: "/"
    });

    //$locationProvider.html5Mode(false).hashPrefix("!");
}]).config(['$provide', function ($provide) {
    $provide.decorator('taOptions', ['taRegisterTool', '$delegate', function (taRegisterTool, taOptions) {

        taOptions.forceTextAngularSanitize = false;

        return taOptions;
    }]);
}]);

app.run(['$rootScope', '$window', '$anchorScroll', 'UserService', 'DTDefaultOptions', 'toastrConfig', 'blockUIConfig',
    function ($rootScope, $window, $anchorScroll, UserService, DTDefaultOptions, toastrConfig, blockUIConfig) {

        $rootScope.$on('$routeChangeError', function (e, curr, prev) {
            e.preventDefault();
        });

        // Scroll top when route change.
        $rootScope.$on("$locationChangeStart", function (e, curr, prev) {
            $anchorScroll();
        });

        // Scroll top when route change.
        $rootScope.$on("$routeChangeStart", function (e, curr, prev) {
            $rootScope.Page = {
                title: curr.$$route.title,
                activeTab: curr.$$route.activeTab,
                breadcrumb: curr.$$route.breadcrumb
            }
            //        ShareData.currentPage =

        });

        DTDefaultOptions.setLanguage({
            "sEmptyTable": "Không có dữ liệu",
            "sInfo": "Hiển thị từ _START_ tới _END_ của _TOTAL_",
            "sInfoEmpty": "Hiển thị từ 0 tới 0 của 0",
            "sInfoFiltered": "(Lọc từ tổng số _MAX_ dữ liệu)",
            "sInfoPostFix": "",
            "sInfoThousands": ",",
            "sLengthMenu": "Hiển thị _MENU_",
            "sLoadingRecords": "Đang tải...",
            "sProcessing": "Đang xử lí...",
            "sSearch": "Tìm kiếm:",
            "sZeroRecords": "Không tìm thấy",
            "oPaginate": {
                "sFirst": "Đầu",
                "sLast": "Cuối",
                "sNext": "Tiếp",
                "sPrevious": "Trước"
            },
            "oAria": {
                "sSortAscending": ": activate to sort column ascending",
                "sSortDescending": ": activate to sort column descending"
            }
        });

        // Config angular-blockui.
        blockUIConfig.delay = 100;
        blockUIConfig.blockBrowserNavigation = true;

        // Config angular toarstr.
        angular.extend(toastrConfig, {
            maxOpened: 1,
            closeButton: true,
            newestOnTop: true,
            autoDismiss: true
        });

        // Base Url of web app.
        $rootScope.BaseUrl = angular.element($('#BaseUrl')).val();

        // Load authen info:
        $rootScope.UserInfo = {
            IsAuthen: false
        };
        // 1. define function
        function checkLoginStatus() {
            var promiseGet = UserService.checkLoginStatus();
            promiseGet.then(
                function (result) {
                    if (result.data.Status === "success") {
                        // Save authen info into $rootScope
                        $rootScope.UserInfo = result.data.Data;
                        $rootScope.UserInfo.IsAuthen = true;
                    } else {
                        $rootScope.UserInfo = {
                            IsAuthen: false
                        };
                    }
                },
                function (error) {
                    // todo here.
                });
        }
        // 2. call function
        //checkLoginStatus();
    }]);