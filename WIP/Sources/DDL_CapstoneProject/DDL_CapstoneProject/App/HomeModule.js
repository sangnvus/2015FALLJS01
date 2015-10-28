"use strict";
var service = angular.module("DDLService", []);
var directive = angular.module("DDLDirective", []);
var app = angular.module("ClientApp", ["ngRoute", "ngAnimate", "ngSanitize", "DDLService",
    "DDLDirective", 'angular-loading-bar', 'textAngular', 'toastr', 'ui.bootstrap', 'monospaced.elastic',
    'datatables', 'datatables.bootstrap', 'oitozero.ngSweetAlert', 'angular.morris-chart', 'blockUI', 'chart.js','ui.select']);

// Show Routing.
app.config(["$routeProvider", function ($routeProvider) {
    $routeProvider.when("/home",
        {
            caseInsensitiveMatch: true,
            redirectTo: "/"
        });
    $routeProvider.when("/",
        {
            caseInsensitiveMatch: true,
            templateUrl: "/ClientPartial/Home",
            controller: 'HomeController',
            resolve: {
                slides: ['SlideService', function (SlideService) {
                    return SlideService.getSlides();
                }],
                liststatisticforhome: ['ProjectService', function (ProjectService) {
                    return ProjectService.GetStatisticListForHome();
                }],
            }
        });
    $routeProvider.when("/discover",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/Discover",
            controller: 'DiscoverController',
            resolve: {
                projectstatisticlist: ['ProjectService', function (ProjectService) {
                    return ProjectService.GetProjectStatisticList();
                }],
                categoryprojectcount: ['CategoryService', function (CategoryService) {
                    return CategoryService.GetCategoryProjectCount();
                }]
            }
        });
    $routeProvider.when("/statistics",
        {
            templateUrl: "ClientPartial/Statistics",
            controller: 'StatisticsController',
            resolve: {
                projectSucesedCount: ['ProjectService', function (ProjectService) {
                    return ProjectService.getStatisticsInfor();
                }],
                projectTopList: ['ProjectService', function (ProjectService) {
                    return ProjectService.getProjectTop("All");
                }],
                categoryStatistic: ['CategoryService', function (CategoryService) {
                    return CategoryService.listDataForStatistic();
                }],
                UserTopList: ['UserService', function (UserService) {
                    return UserService.getUserTop("All");
                }],
            }
        });

    $routeProvider.when("/search",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/Search",
            controller: 'SearchController',
            resolve: {
                projectbycategory: ['ProjectService', 'CategoryService', '$route', function (ProjectService, CategoryService, $route) {
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
                    var projectList = ProjectService.SearchProject(0, "|" + params.categoryid + "|", params.order, searchkey);
                    return projectList;
                }],
                categoryList: ['CategoryService', function (CategoryService) {
                    return CategoryService.getAllCategories();
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

            }
        });
    $routeProvider.when("/register",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/Register",
            controller: "RegisterController"
        });
    $routeProvider.when("/register_success",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/RegisterSuccess"
        });

    $routeProvider.when("/user/message",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/Message",
            controller: "MessageController",
            resolve: {
                conversations: ['$route', '$rootScope', '$q', 'MessageService', 'CommmonService', function ($route, $rootScope, $q, MessageService, CommmonService) {
                    var promise;
                    if ($route.current.params.list == null || $route.current.params.list !== "sent") {
                        promise = MessageService.getListReceivedConversations();
                    } else {
                        promise = MessageService.getListSentConversations();
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
            resolve: {
                userpublicinfo: ['$route', 'UserService', function ($route, UserService) {
                    return UserService.getPublicInformation($route.current.params.username);
                }]
            }
        });

    $routeProvider.when("/user/editprofile",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/EditProfile",
            controller: 'EditProfileController',
            resolve: {
                usereditinfo: ['$rootScope', '$route', 'UserService', '$q', 'CommmonService', function ($rootScope, $route, UserService, $q, CommmonService) {
                    var promise = UserService.getProfileInformation();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });
    $routeProvider.when("/project/backedProject",
         {
             caseInsensitiveMatch: true,
             templateUrl: "ClientPartial/BackedProject",
             controller: 'BackedProjectController',
             resolve: {
                 listsBacked: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                     var promise = ProjectService.getBackedProject();
                     return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                 }]
             }
         });

    $routeProvider.when("/project/listBacker",
        {
            caseInsensitiveMatch: true,
            templateUrl: "ClientPartial/ListBacker",
            controller: 'ListBackerController',
            resolve: {
                projects: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                    var promise = ProjectService.getListBacker();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });

    $routeProvider.when("/project/starredProject",
       {
           caseInsensitiveMatch: true,
           templateUrl: "ClientPartial/StarredProject",
           controller: 'StarredProjectController',
           resolve: {
               project: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                   var promise = ProjectService.getStarredProject();
                   return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
               }]
           }
       });

    $routeProvider.when("/project/createdProject",
      {
          caseInsensitiveMatch: true,
          templateUrl: "ClientPartial/CreatedProject",
          controller: 'CreatedProjectController',
          resolve: {
              projects: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                  var promise = ProjectService.getCreatedProject();
                  return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
              }]
          }
      });

    $routeProvider.when("/project/backedProjectHistory",
      {
          templateUrl: "ClientPartial/BackedProjectHistory",
          controller: 'BackedHistoryProjectController',
          resolve: {
              projects: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                  var promise = ProjectService.getBackedProjectHistory();
                  return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
              }]
          }
      });

    $routeProvider.when("/slide",
        {
            templateUrl: "/AdminPartial/Slide",
            controller: 'AdminSlideController',
            activeTab: 'slide',
            breadcrumb: ['Quản lý Slide', 'Danh sách Slide'],
            title: 'Quản lý Slide',
            resolve: {
                slides: ['$rootScope', '$q', 'AdminSlideService', 'CommmonService', function ($rootScope, $q, AdminSlideService, CommmonService) {
                    var promise = AdminSlideService.getSlides();
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
}]).config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
    cfpLoadingBarProvider.includeSpinner = false;
}]);

app.run(['$rootScope', '$window', '$anchorScroll', 'UserService', 'DTDefaultOptions', 'toastrConfig', 'blockUIConfig', 'MessageService',
    function ($rootScope, $window, $anchorScroll, UserService, DTDefaultOptions, toastrConfig, blockUIConfig, MessageService) {
        $rootScope.$on('$routeChangeError', function (e, curr, prev) {
            e.preventDefault();
        });

        // Scroll top when route change.
        $rootScope.$on("$locationChangeStart", function () {
            $anchorScroll();
        });

        //$rootScope.$on('$routeChangeSuccess', function (e, curr, prev) {
            //if ($rootScope.UserInfo.IsAuthen === true) {
            //    var promiseGet = MessageService.getNumberNewMessage();
            //    promiseGet.then(
            //        function (result) {
            //            if (result.data.Status === "success") {
            //                // Save authen info into $rootScope
            //                $rootScope.UserInfo.NumberNewMessage = result.data.Data;
            //                $rootScope.UserInfo.NumberNewMessage.Total = result.data.Data.ReceivedMessage + result.data.Data.SentMessage;
            //            } else {
            //                $rootScope.UserInfo.NumberNewMessage = 0;
            //            }
            //        },
            //        function (error) {
            //            $rootScope.UserInfo.NumberNewMessage = 0;
            //        });
            //}
        //});

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
            progressBar: true
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
    }]);
