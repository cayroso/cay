
var app = angular.module('app', ['ngRoute', 'ngCookies', 'ui.router', 'http-auth-interceptor', 'myLoginCheck']);

app.controller('appController',
    ['$scope', '$http', '$state',
        'sessionSvc', 'authService', 'Auth',
function ($scope, $http, $state,
        sessionSvc, authService, Auth) {

    function init() {
        //$scope.$root.isAuthenticated = Auth.user().sid === undefined ? false : Auth.user().sid != 0;
        $scope.userName = 'admin';
        $scope.password = '1234';
        $scope.loginError = '';
        $scope.url = 'request';

        $scope.login = $('#login-holder');
        $scope.main = $('#content');

        //var _isAuthenticated = false;
        $scope.isAuthenticated = function () {
            return Auth.user() === undefined ? false : Auth.user().sid != 0;
        };
        //$scope.session = sessionSvc;
        //alert('Welcome, [' + $scope.session.userName + ']');


        // watch the service for changes to currentUser
        //$scope.$watch(function () {
        //    return Auth.user();
        //}, function (auth) {
        //    $scope._isAuthenticated = auth.sid === undefined ? false : auth.sid != 0;
        //}, true);

        //$scope.$watch(function () {
        //    return sessionSvc.isAuthenticated;
        //}, function (isAuthenticated) {
        //    $scope.isAuthenticated = isAuthenticated;
        //}, true);
    };

    $scope.logout = function () {
        $http.post('auth/logout').success(function () {
            Auth.remove();
            alert('you are now logged out');
            $state.transitionTo("home");
        });
    };

    $scope.getResource = function (url) {
        $http.get(url).success(function (data, status) {
            alert('you got the request: ' + JSON.stringify(data, undefined, 2));
        }).error(function (data, status) {
            alert('data: ' + JSON.stringify(data, undefined, 2));
        })
        ;
    };




    init();

}]);

app.directive('authDemoApplication', function () {
    return {
        restrict: 'C',
        link: function (scope, elem, attrs) {
            //once Angular is started, remove class:
            ///elem.removeClass('waiting-for-angular');

            //var login = elem.find('#login-holder');
            //var main = elem.find('#content');
            scope.login.hide();

            scope.$on('event:auth-loginRequired', function (ev, msg) {
                scope.login.slideDown('slow', function () {
                    //debugger;
                    scope.main.hide();
                    scope.login.show();
                    //login.slideUp();
                    if (msg.data.responseStatus != undefined)
                        scope.loginError = msg.data.responseStatus.message;

                });
            });
            scope.$on('event:auth-loginConfirmed', function () {
                //main.show();
                //login.hide();//.slideUp();
                scope.main.show();
                scope.login.hide();
            });
        }
    }
});

//This configures the routes and associates each route with a view and a controller
app.config(['$stateProvider', '$urlRouterProvider', '$locationProvider',
    function ($stateProvider, $urlRouterProvider, $locationProvider) {
        'use strict';
       
        $urlRouterProvider.otherwise('/');

        $stateProvider
            .state('home', {
                url: '/'
                //templateUrl: 'apps/common/views/login.html',
                //controller: 'loginController'
            })
            .state('login', {
                url: '/login?redirect&errors',
                templateUrl: 'apps/common/views/login.html',
                controller: 'loginController'
            })
            .state('adminPanel', {
                url: '/admin',
                templateUrl: 'apps/admin/views/adminPanel.html'
            })

            .state('adminPanel.users', {
                url: '/users',
                templateUrl: 'apps/admin/views/account/users.html',
                controller: 'usersController'
            })
            .state('adminPanel.users.create', {
                url: '/create',
                templateUrl: 'apps/admin/views/account/users.create.html'
            })
            .state('adminPanel.users.search', {
                url: '/search',
                templateUrl: 'apps/admin/views/account/users.search.html'
            })
        ;

    }]);
app.run(['$cookieStore', 'Auth', function run($cookieStore, Auth) {

    //var _user = $cookieStore.get('current.user') || { sid: 0 };//= UserRestService.requestCurrentUser();
    //alert(JSON.stringify(_user, undefined, 3));
    //Auth.set(_user);
}]);