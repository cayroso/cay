angular.module('myLoginCheck', []).factory('Auth', ['$cookieStore', function ($cookieStore) {
    'user strict';

    var foo = function () {
        //var _user;
        return {
            user: function () {
                return $cookieStore.get('current.user');
            },
            set: function (__user) {
                // you can retrive a user setted from another page, like login sucessful page.
                existing_cookie_user = $cookieStore.get('current.user');
                __user = __user || existing_cookie_user;
                $cookieStore.put('current.user', __user);
            },

            remove: function () {
                $cookieStore.remove('current.user');
                //_user = {};
            }
        }
    };

    return new foo();

    //var _user = {};
    //return {

    //    user: function () { return _user },

    //    set: function (__user) {
    //        // you can retrive a user setted from another page, like login sucessful page.
    //        existing_cookie_user = $cookieStore.get('current.user');
    //        _user = __user || existing_cookie_user;
    //        $cookieStore.put('current.user', _user);
    //    },

    //    remove: function () {
    //        $cookieStore.remove('current.user', _user);
    //        _user = {};
    //    }
    //};
}]);

//angular.module('myLoginCheck', []).service('Auth', ['$cookieStore', function ($cookieStore) {
//    'user strict';

//    var _user = {};

//    this.user = function () {
//        return _user;
//    };

//    this.set = function (__user) {
//        // you can retrive a user setted from another page, like login sucessful page.
//        existing_cookie_user = $cookieStore.get('current.user');
//        _user = __user || existing_cookie_user;
//        $cookieStore.put('current.user', _user);
//    };

//    this.remove = function () {
//        $cookieStore.remove('current.user', _user);
//    };
//}]);

app.controller('loginController',
['$scope', '$stateParams', '$http', '$state',
    'sessionSvc', 'authService', 'Auth',
function ($scope, $stateParams, $http, $state,
    sessionSvc, authService, Auth) {

    function vars() {
        $scope.title = 'The quick brown fox';
        $scope.redirect = $stateParams.redirect || '';
        $scope.errors = $stateParams.errors || 'none';
    };

    function init() {
        vars();
    };

    $scope.submit = function () {

        $scope.loginError = '';

        $http({
            ignoreAuthModule: true,
            method: 'POST',
            url: 'auth/credentials?username=' + $scope.userName + '&password=' + $scope.password
        })
        .success(function (data, status) {
            //$scope.userName = '';
            //$scope.password = '';

            sessionSvc.sessionId = data.sessionId;
            sessionSvc.userName = data.userName;
            //$scope.$root.isAuthenticated = true;

            var user = { sid: data.sessionId, userName: data.userName, authenticated: true };
            Auth.set(user);
            //Auth.set(user);

            authService.loginConfirmed();
            //$state.transitionTo("home");

        }).error(function (data, status) {
            sessionSvc.sessionId = 0;
            sessionSvc.userName = '';
            $scope.isAuthenticated = false;

            $scope.loginError = JSON.stringify(data, undefined, 3)

            Auth.remove();
        })
        ;
    }


    init();

}]);
