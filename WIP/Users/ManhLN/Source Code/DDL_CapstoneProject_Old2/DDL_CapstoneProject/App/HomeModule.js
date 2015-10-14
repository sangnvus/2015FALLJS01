"use strict";
var service = angular.module("DDLService", []);
var directive = angular.module("DDLDirective", []);
var app = angular.module("ClientApp", ["ngRoute", "ngAnimate", "ngSanitize", "DDLService",
    "DDLDirective", 'angular-loading-bar', 'textAngular', 'toastr', 'ui.bootstrap', 'monospaced.elastic','datatables','datatables.bootstrap']);

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
                conversations: ['$rootScope', '$q', 'MessageService', 'CommmonService', function ($rootScope, $q, MessageService, CommmonService) {
                    var promise = MessageService.getListReceivedConversations();
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
    $routeProvider.when("/project/create",
        {
            templateUrl: "/ClientPartial/CreateProject",
            controller: "CreateProjectController",
            resolve: {
                categories: ['$rootScope', '$route', '$q', 'CategoryService', 'CommmonService', function ($rootScope, $route, $q, CategoryService, CommmonService) {
                    var promise = CategoryService.getCategories();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });
    $routeProvider.when("/project/edit/:id",
        {
            templateUrl: "/ClientPartial/EditProject",
            controller: "EditProjectController",
            resolve: {
                categories: ['$rootScope', '$route', '$q', 'CategoryService', 'CommmonService', function ($rootScope, $route, $q, CategoryService, CommmonService) {
                    var promise = CategoryService.getCategories();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
                project: ['$rootScope', '$route', '$q', 'ProjectService', 'CommmonService', function ($rootScope, $route, $q, ProjectService, CommmonService) {
                    var promise = ProjectService.getProject($route.current.params.id);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }],
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
                usereditinfo: ['$route', 'UserService', function ($route, UserService) {
                    return UserService.getProfileInformation($route.current.params.username);
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

app.run(['$rootScope', '$window', '$anchorScroll', 'UserService', function ($rootScope, $window, $anchorScroll, UserService) {
    $rootScope.$on('$routeChangeError', function (e, curr, prev) {
        e.preventDefault();
    });

    // Scroll top when route change.
    $rootScope.$on("$locationChangeStart", function () {
        $anchorScroll();
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