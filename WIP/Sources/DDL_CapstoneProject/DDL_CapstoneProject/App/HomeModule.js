"use strict";
var service = angular.module("DDLService", []);
var directive = angular.module("DDLDirective", []);
var app = angular.module("ClientApp", ["ngRoute", "ngAnimate", "ngSanitize", "DDLService",
    "DDLDirective", 'angular-loading-bar', 'textAngular', 'toastr', 'ui.bootstrap', 'monospaced.elastic',
    'datatables', 'datatables.bootstrap', 'oitozero.ngSweetAlert','angular.morris-chart']);

// Show Routing.
app.config(["$routeProvider", function ($routeProvider) {
    $routeProvider.when("/home",
        {
            redirectTo: "/"
        });
    $routeProvider.when("/",
        {
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
            templateUrl: "ClientPartial/Discover",
            controller: 'DiscoverController',
            resolve: {
                projectstatisticlist: ['ProjectService', function (ProjectService) {
                    return ProjectService.GetProjectStatisticList();
                }],
                //popularprojectwithcategory: ['ProjectService', function (ProjectService) {
                //    return ProjectService.GetProjectByCategory();
                //}],
                categoryprojectcount: ['CategoryService', function (CategoryService) {
                    return CategoryService.GetCategoryProjectCount();
                }]
            }
        });
    $routeProvider.when("/register",
        {
            templateUrl: "ClientPartial/Register",
            controller: "RegisterController"
        });
    $routeProvider.when("/register_success",
        {
            templateUrl: "ClientPartial/RegisterSuccess"
        });

    $routeProvider.when("/user/message",
        {
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
            templateUrl: "ClientPartial/MessageDetail",
            controller: "MessageDetailController",
            resolve: {
                conversation: ['$rootScope', '$route', '$q', 'MessageService', 'CommmonService', function ($rootScope, $route, $q, MessageService, CommmonService) {
                    var promise = MessageService.getConversation($route.current.params.id);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });
    $routeProvider.when("/project/create", {
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
            templateUrl: "/ClientPartial/EditProject",
            controller: "EditProjectController",
            resolve: {
                categories: ['$rootScope', '$route', '$q', 'CategoryService', 'CommmonService', function ($rootScope, $route, $q, CategoryService, CommmonService) {
                    var promise = CategoryService.getCategories();
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
            templateUrl: "ClientPartial/BackProject",
            controller: "BackProjectController",
            resolve: {
                rewardPkgs: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                    var promise = ProjectService.getRewardPkgByCode($route.current.params.code);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });
    $routeProvider.when("/project/detail/:code",
        {
            templateUrl: "ClientPartial/ProjectDetail",
            controller: "ProjectDetailController",
            resolve: {
                project: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                    var promise = ProjectService.getProjectDetail($route.current.params.code);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
                listbacker: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                    var promise = ProjectService.getListBacker($route.current.params.code);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });

    $routeProvider.when("/user/publicprofile/:username",
        {
            templateUrl: "ClientPartial/PublicProfile",
            controller: 'PublicProfileController',
            resolve: {
                userpublicinfo: ['$route', 'UserService', function ($route, UserService) {
                    return UserService.getPublicInformation($route.current.params.username);
                }]
            }
        });

    $routeProvider.when("/user/editprofile/:username",
        {
            templateUrl: "ClientPartial/EditProfile",
            controller: 'EditProfileController',
            resolve: {
                usereditinfo: ['$rootScope', '$route', 'UserService', '$q', 'CommmonService', function ($rootScope, $route, UserService, $q, CommmonService) {
                    var promise = UserService.getProfileInformation($route.current.params.username);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });
  $routeProvider.when("/project/backedProject",
       {
           templateUrl: "ClientPartial/BackedProject",
           controller: 'BackedProjectController',
            resolve: {
                listsBacked: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                    var promise = ProjectService.getBackedProject();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
       });

    $routeProvider.when("/project/starredProject",
       {
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
          templateUrl: "ClientPartial/CreatedProject",
          controller: 'CreatedProjectController',
          resolve: {
              projects: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                  var promise = ProjectService.getCreatedProject();
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

app.run(['$rootScope', '$window', '$anchorScroll', 'UserService', 'DTDefaultOptions', function ($rootScope, $window, $anchorScroll, UserService, DTDefaultOptions) {
    $rootScope.$on('$routeChangeError', function (e, curr, prev) {
        e.preventDefault();
    });

    // Scroll top when route change.
    $rootScope.$on("$locationChangeStart", function () {
        $anchorScroll();
    });

    // Set language for table
    DTDefaultOptions.setLanguage({
        "sEmptyTable": "Không có dữ liệu",
        "sInfo": "Hiển thị từ _START_ tới _END_ của _TOTAL_",
        "sInfoEmpty": "Hiển thị từ 0 tới 0 của 0",
        "sInfoFiltered": "(filtered from _MAX_ total entries)",
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