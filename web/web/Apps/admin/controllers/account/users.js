
app.controller('usersController',
['$scope', '$stateParams', '$http',
    'Auth',
function ($scope, $stateParams, $http,
    Auth) {

    function vars() {
        $scope.title = $stateParams.title || 'The quick brown fox';
        $scope.dataFromUsers = 'invalid';
    };

    function init() {

        if (Auth.user() === undefined || Auth.user().sid === 0)
            return;

        vars();

        return $http.get('authorized/users').success(function (data, status) {
            $scope.dataFromUsers = 'this is from /users';
            //alert('succes');
        }).error(function (data, status) {
            alert('error');
        });
        //alert($scope.title);
    };

    init();

}]);
