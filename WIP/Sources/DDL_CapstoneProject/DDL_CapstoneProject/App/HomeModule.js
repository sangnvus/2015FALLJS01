"use strict";
var service = angular.module("DDLService", []);
var directive = angular.module("DDLDirective", []);
var app = angular.module("ClientApp", ["ngRoute", "ngAnimate", "ngSanitize", "DDLService",
    "DDLDirective", 'angular-loading-bar', 'textAngular', 'toastr', 'ui.bootstrap', 'monospaced.elastic',
    'datatables', 'datatables.bootstrap', 'oitozero.ngSweetAlert', 'angular.morris-chart',
    'ChartAngular', 'blockUI', 'chart.js', 'ui.select']);

// Show Routing.
app.config(["$routeProvider", function ($routeProvider) {
    $routeProvider.when("/home",
        {
            caseInsensitiveMatch: true,
            redirectTo: "/",
            title: 'Dandelion',
        });
    $routeProvider.when("/",
        {
            caseInsensitiveMatch: true,
            templateUrl: "/ClientPartial/Home",
            controller: 'HomeController',
            title: 'Dandelion',
            resolve: {
                slides: ['SlideService', 'CommmonService', '$rootScope', '$q', function (SlideService, CommmonService, $rootScope, $q) {
                    var promise = SlideService.getSlides();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
                liststatisticforhome: ['ProjectService', 'CommmonService', '$rootScope', '$q', function (ProjectService, CommmonService, $rootScope, $q) {
                    var promise = ProjectService.GetStatisticListForHome();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
            }
        });
    $routeProvider.when("/discover",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/Discover",
            controller: 'DiscoverController',
            title: 'Danh mục dự án - Dandelion',
            resolve: {
                projectstatisticlist: ['ProjectService', 'CommmonService', '$rootScope', '$q', function (ProjectService, CommmonService, $rootScope, $q) {
                    var promise = ProjectService.GetProjectStatisticList();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
                categoryprojectcount: ['CategoryService', 'CommmonService', '$rootScope', '$q', function (CategoryService, CommmonService, $rootScope, $q) {
                    var promise = CategoryService.GetCategoryProjectCount();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });
    $routeProvider.when("/statistics",
        {
            templateUrl: "ClientPartial/Statistics",
            controller: 'StatisticsController',
            title: 'Thống kê - Dandelion',
            resolve: {
                projectSucesedCount: ['ProjectService', 'CommmonService', '$rootScope', '$q', function (ProjectService, CommmonService, $rootScope, $q) {
                    var promise = ProjectService.getStatisticsInfor();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
                projectTopList: ['ProjectService', 'CommmonService', '$rootScope', '$q', function (ProjectService, CommmonService, $rootScope, $q) {
                    var promise = ProjectService.getProjectTop("All");
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
                categoryStatistic: ['CategoryService', 'CommmonService', '$rootScope', '$q', function (CategoryService, CommmonService, $rootScope, $q) {
                    var promise = CategoryService.listDataForStatistic();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
                UserTopList: ['UserService', 'CommmonService', '$rootScope', '$q', function (UserService, CommmonService, $rootScope, $q) {
                    var promise = UserService.getUserTop("All");
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
            }
        });

    $routeProvider.when("/search",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/Search",
            controller: 'SearchController',
            title: 'Tìm kiếm- Dandelion',
            resolve: {
                projectbycategory: ['ProjectService', 'CategoryService', '$route', '$q', '$rootScope', 'CommmonService', function (ProjectService, CategoryService, $route, $q, $rootScope, CommmonService) {
                    var params = $route.current.params;
                    if (typeof (params.categoryid) == "undefined") {
                        params.categoryid = "all";
                    }
                    if (typeof (params.order) == "undefined") {
                        params.order = "PopularPoint";
                    }
                    var searchkey = params.searchkey;
                    if (typeof (params.searchkey) == "undefined") {
                        searchkey = "null";
                        params.searchkey = [""];
                    }

                    var promise = ProjectService.SearchProject(0, "|" + params.categoryid + "|", params.order, searchkey, "true");
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
                categoryList: ['$rootScope', 'CategoryService', 'CommmonService', '$q', function ($rootScope, CategoryService, CommmonService, $q) {
                    var promise = CategoryService.getAllCategories();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
                isAdvance: ['$route', function ($route) {
                    var params = $route.current.params;
                    if (params.advance) {
                        return true;
                    } else {
                        return false;
                    }
                }],
                selectedorder: ['$route', function ($route) {
                    return $route.current.params.order;
                }],
                selectcategory: ['$route', function ($route) {
                    return $route.current.params.categoryid;
                }],
                searchkey: ['$route', function ($route) {
                    return $route.current.params.searchkey;
                }],
                projectResultListSize: ['ProjectService', '$route', '$q', '$rootScope', 'CommmonService', function (ProjectService, $route, $q, $rootScope, CommmonService) {
                    var promise = ProjectService.SearchCount("|" + $route.current.params.categoryid + "|", $route.current.params.searchkey, "true");
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],

            }
        });
    $routeProvider.when("/register",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/Register",
            controller: "RegisterController",
            title: 'Đăng ký - Dandelion',
        });
    $routeProvider.when("/register_success",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/RegisterSuccess",
            title: 'Đăng ký thành công - Dandelion',
        });

    $routeProvider.when("/user/message",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/Message",
            controller: "MessageController",
            title: 'Danh sách tin nhắn - Dandelion',
            resolve: {
                conversations: ['$route', '$rootScope', '$q', 'MessageService', 'CommmonService', function ($route, $rootScope, $q, MessageService, CommmonService) {
                    var promise;
                    if ($route.current.params.list === "inbox") {
                        promise = MessageService.getListReceivedConversations();
                    } else if ($route.current.params.list === "sent") {
                        promise = MessageService.getListSentConversations();
                    } else {
                        promise = MessageService.getListConversations();
                    }
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });
    $routeProvider.when("/user/message/:id",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/MessageDetail",
            controller: "MessageDetailController",
            title: 'Tin nhắn - Dandelion',
            resolve: {
                conversation: ['$rootScope', '$route', '$q', 'MessageService', 'CommmonService', function ($rootScope, $route, $q, MessageService, CommmonService) {
                    var promise = MessageService.getConversation($route.current.params.id);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });
    $routeProvider.when("/project/create",
        {
            caseInsensitiveMatch: true,
            templateUrl: "/ClientPartial/CreateProject",
            controller: "CreateProjectController",
            title: 'Tạo dự án - Dandelion',
            resolve: {
                categories: ['$rootScope', '$route', '$q', 'CategoryService', 'CommmonService', function ($rootScope, $route, $q, CategoryService, CommmonService) {
                    var promise = CategoryService.GetCategoriesForCreate();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });
    $routeProvider.when("/project/edit/:code",
        {
            caseInsensitiveMatch: true,
            templateUrl: "/ClientPartial/EditProject",
            controller: "EditProjectController",
            title: 'Chỉnh sửa dự án - Dandelion',
            resolve: {
                categories: ['$rootScope', '$route', '$q', 'CategoryService', 'CommmonService', function ($rootScope, $route, $q, CategoryService, CommmonService) {
                    var promise = CategoryService.getAllCategories();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
                project: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                    var promise = ProjectService.getProject($route.current.params.code);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
            }
        });
    $routeProvider.when("/project/back/:code",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/BackProject",
            controller: "BackProjectController",
            title: 'Ủng hộ dự án - Dandelion',
            resolve: {
                rewardPkgs: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                    var promise = ProjectService.getRewardPkgByCode($route.current.params.code);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
                project: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                    var promise = ProjectService.getBackProjectInfo($route.current.params.code);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
            }
        });
    $routeProvider.when("/project/payment/:code",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/PaymentProject",
            controller: "PaymentProjectController",
            title: 'Ủng hộ dự án - Dandelion',
            resolve: {
                rewardPkgs: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                    var promise = ProjectService.getRewardPkgByCode($route.current.params.code);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
                usereditinfo: ['$rootScope', '$route', 'UserService', '$q', 'CommmonService', function ($rootScope, $route, UserService, $q, CommmonService) {
                    var promise = UserService.getProfileInformation();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
                project: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                    var promise = ProjectService.getBackProjectInfo($route.current.params.code);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
            }
        });
    $routeProvider.when("/project/detail/:code",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/ProjectDetail",
            controller: "ProjectDetailController",
            title: 'Thông tin dự án - Dandelion',
            resolve: {
                project: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                    var promise = ProjectService.getProjectDetail($route.current.params.code);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });

    $routeProvider.when("/user/editpassword/",
        {
            templateUrl: "ClientPartial/EditPassword",
            controller: 'EditPasswordController',
            title: 'Tài khoản - Dandelion',
            resolve: {
                userpublicinfo: ['$rootScope', '$route', 'UserService', '$q', 'CommmonService', function ($rootScope, $route, UserService, $q, CommmonService) {
                    var promise = UserService.getEditPassword();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });

    $routeProvider.when("/user/publicprofile/:username",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/PublicProfile",
            controller: 'PublicProfileController',
            title: 'Thông tin người dùng - Dandelion',
            resolve: {
                userpublicinfo: ['$rootScope', '$route', 'UserService', '$q', 'CommmonService', function ($rootScope, $route, UserService, $q, CommmonService) {
                    var promise = UserService.getPublicInformation($route.current.params.username);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });

    $routeProvider.when("/user/editprofile",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/EditProfile",
            controller: 'EditProfileController',
            title: 'Thông tin cá nhân - Dandelion',
            resolve: {
                usereditinfo: ['$rootScope', '$route', 'UserService', '$q', 'CommmonService', function ($rootScope, $route, UserService, $q, CommmonService) {
                    var promise = UserService.getProfileInformation();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });
    $routeProvider.when("/project/backed",
         {
             caseInsensitiveMatch: true,
             templateUrl: "ClientPartial/BackedProject",
             controller: 'BackedProjectController',
             title: 'Dự án đã ủng hộ - Dandelion',
             resolve: {
                 listsBacked: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                     var promise = ProjectService.getBackedProject();
                     return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                 }]
             }
         });

    $routeProvider.when("/project/listBacker/:code",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/ListBacker",
            controller: 'ListBackerController',
            title: 'Danh sách ủng hộ - Dandelion',
            resolve: {
                projects: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                    var promise = ProjectService.getListBacker($route.current.params.code);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });

    $routeProvider.when("/project/reminded",
       {
           caseInsensitiveMatch: true,
           templateUrl: "ClientPartial/StarredProject",
           controller: 'StarredProjectController',
           title: 'Dự án đang theo dõi - Dandelion',
           resolve: {
               project: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                   var promise = ProjectService.getStarredProject();
                   return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
               }]
           }
       });

    $routeProvider.when("/project/created",
      {
          caseInsensitiveMatch: true,
          templateUrl: "ClientPartial/CreatedProject",
          controller: 'CreatedProjectController',
          title: 'Thông tin đã tạo - Dandelion',
          resolve: {
              projects: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                  var promise = ProjectService.getCreatedProject();
                  return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
              }]
          }
      });

    $routeProvider.when("/project/backhistory",
    {
        templateUrl: "ClientPartial/BackedProjectHistory",
        controller: 'BackedHistoryProjectController',
        title: 'Lịch sử ủng hộ - Dandelion',
        resolve: {
            projects: [
                '$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function($rootScope, $route, $q, ProjectService, CommmonService) {
                    var promise = ProjectService.getBackedProjectHistory();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }
            ]
        }
    });

    $routeProvider.when("/error",
        {
            caseInsensitiveMatch: true,
            templateUrl: "/ClientPartial/Error",
            title: 'Lỗi - Dandelion',
        });
    $routeProvider.when("/notfound",
        {
            caseInsensitiveMatch: true,
            title: 'Không tìm thấy - Dandelion',
            templateUrl: "/ClientPartial/NotFound"
        });
    $routeProvider.otherwise({
        redirectTo: "/",
        title: 'Dandelion',
    });

    //$locationProvider.html5Mode(false).hashPrefix("!");
}]).config(['$provide', function ($provide) {
    $provide.decorator('taOptions', ['taRegisterTool', '$delegate', function (taRegisterTool, taOptions) {

        taOptions.forceTextAngularSanitize = false;

        return taOptions;
    }]);
}]).config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
    cfpLoadingBarProvider.includeSpinner = false;
}]).config(['ChartJsProvider', function (ChartJsProvider) {
    //// Configure all charts
    //ChartJsProvider.setOptions({
    //    colours: ['#FF5252', '#FF8A80'],
    //    responsive: false
    //});
    // Configure all line charts
    ChartJsProvider.setOptions('Line', {
        datasetFill: false
    });
}]);

app.run(['$rootScope', '$window','$location','$route', '$anchorScroll', 'UserService', 'DTDefaultOptions', 'toastrConfig', 'blockUIConfig', 'MessageService',
    function ($rootScope, $window,$location,$route, $anchorScroll, UserService, DTDefaultOptions, toastrConfig, blockUIConfig, MessageService) {
        $rootScope.$on('$routeChangeError', function (e, curr, prev) {
            e.preventDefault();
        });

        // Scroll top when route change.
        $rootScope.$on("$viewContentLoaded", function () {
            $window.scrollTo(0, 0);
        });

        $rootScope.reload = function() {
            $route.reload();
        }

        $rootScope.$on("$routeChangeStart", function (e, curr, prev) {
            if (curr.$$route !== undefined) {
                $rootScope.Page = {
                    title: curr.$$route.title !== undefined ? curr.$$route.title : ""
                }
            }

        });

        $rootScope.$on('$routeChangeSuccess', function (e, curr, prev) {
            if (curr.$$route.redirectTo === undefined) {
                if ($rootScope.UserInfo.IsAuthen === true) {
                    var promiseGet = MessageService.getNumberNewMessage();
                    promiseGet.then(
                        function (result) {
                            if (result.data.Status === "success") {
                                //Save new message number into $rootScope
                                $rootScope.UserInfo.NumberNewMessage = result.data.Data;
                                $rootScope.UserInfo.NumberNewMessage.Total = result.data.Data.ReceivedMessage + result.data.Data.SentMessage;
                            } else {
                                $rootScope.NumberNewMessage.Total = 0;
                                $rootScope.NumberNewMessage.ReceivedMessage = 0;
                                $rootScope.NumberNewMessage.SentMessage = 0;
                            }
                        },
                        function (error) {
                            $rootScope.NumberNewMessage.Total = 0;
                            $rootScope.NumberNewMessage.ReceivedMessage = 0;
                            $rootScope.NumberNewMessage.SentMessage = 0;
                        });
                }
            }
        });

        // Set language for table
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
        // Tell the blockUI service to ignore certain requests
        blockUIConfig.requestFilter = function (config) {
            // If the request starts with '/api/UserApi/GetListUserName' ...
            if (config.url.match(/^\/api\/UserApi\/GetListUserName($|\/).*/)) {
                return false; // ... don't block it.
            }
        };
        // Config angular toarstr.
        angular.extend(toastrConfig, {
            maxOpened: 2,
            closeButton: true,
            newestOnTop: true,
            autoDismiss: true,
            //progressBar: true
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
        checkLoginStatus();

        $window.fbAsyncInit = function () {
            FB.init({
                appId: '412367302292593',
                status: false,
                cookie: true,
                xfbml: true,
                version: 'v2.4'
            });
            //FbService.watchLoginStatusChange();
        };

        (function (d) {
            // load the Facebook javascript SDK

            var js,
                id = 'facebook-jssdk',
                ref = d.getElementsByTagName('script')[0];

            if (d.getElementById(id)) {
                return;
            }

            js = d.createElement('script');
            js.id = id;
            js.async = true;
            js.src = "//connect.facebook.net/en_US/sdk.js";

            ref.parentNode.insertBefore(js, ref);

        }(document));
    }]);
