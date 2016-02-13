angular.module("umbraco").controller("Skybrud.Social.Facebook.OAuth.Controller", ['$scope', 'editorState', function ($scope, editorState) {

    // Expose the Umbraco version
    $scope.version = window.Umbraco.Sys.ServerVariables.application.version.split('.');

    // Define an alias for the editor (eg. used for callbacks)
    var alias = ('skybrudsocial_' + Math.random()).replace('.', '');

    // Get a reference to the current editor state
    var state = editorState.current;

    $scope.expiresDays = null;
    
    $scope.callback = function (data) {
        $scope.$apply(function () {
            $scope.model.value = data;
            updateUI();
            updateBusinessPagesDropdownUI();
        });
    };


    function updateBusinessPagesDropdownUI() {

        var array = $scope.model.value.business_pages;
        if (array) {
            //for (var i = 0; i < array.length; i++)
            //  console.log(array[i]);
            $scope.accounts = array;
        }

        //Whether there is a selected account, bind data
        if ($scope.model.value && $scope.model.value.selected_business_page) {
            $scope.selectedBusinessPage = $scope.model.value.selected_business_page;
        }

        //Register onChange event
        $scope.businessPageSelected = function (selectedAccount) {
            $scope.selectedBusinessPage = selectedAccount;
            $scope.model.value.selected_business_page = selectedAccount;//persists on Umbraco model
        };
    }

    $scope.authorize = function () {

        var url = '/App_Plugins/Skybrud.Social/Dialogs/FacebookOAuth.aspx?callback=' + alias;
        url += "&contentTypeAlias=" + state.contentTypeAlias;
        url += "&propertyAlias=" + $scope.model.alias;

        window.open(url, 'Facebook OAuth', 'scrollbars=no,resizable=yes,menubar=no,width=1050,height=800');

    };

    $scope.clear = function () {
        $scope.model.value = null;
    };
    
    function format(number, decimals) {
        var text = Math.round(number * 10 * decimals);
        return text / 10 / decimals;
    }
    
    function updateUI() {

        if ($scope.model.value && $scope.model.value.expires_at) {

            var seconds = (new Date($scope.model.value.expires_at) - new Date()) / 1000;
            $scope.expiresDays = format(seconds / 60 / 60 / 24, 2);

        } else {

            $scope.expiresDays = NaN;

        }

    }

    //load stored data in content editor (UI)
    updateUI();
    updateBusinessPagesDropdownUI();

    // Register the callback function in the global scope
    window[alias] = $scope.callback;

}]);

angular.module("umbraco").controller("Skybrud.Social.Facebook.OAuth.PreValues.Controller", ['$scope', '$http', 'dialogService', function ($scope, $http, dialogService) {

    // Expose the Umbraco version
    $scope.version = window.Umbraco.Sys.ServerVariables.application.version.split('.');
    
    if (!$scope.model.value) {
        $scope.model.value = {
            appid: '',
            appsecret: '',
            redirecturi: '',
            scope: []
        };
    }

    if (!$scope.model.value.scope) $scope.model.value.scope = [];

    $scope.addScope = function () {

        var d = dialogService.open({
            modalClass: 'SocialDialog',
            template: '/App_Plugins/Skybrud.Social/Facebook/OAuth/ScopesDialog.html',
            show: true,
            callback: function (scopes) {
                $scope.model.value.scope = scopes;
            },
            selection: $scope.model.value.scope
        });

        d.element[0].style.width = '1000px';
        d.element[0].style.marginLeft = '-500px';

    };

    $scope.removeScope = function (index) {
        $scope.model.value.scope.splice(index, 1);
    };

    $scope.suggestedRedirectUri = window.location.origin + '/App_Plugins/Skybrud.Social/Dialogs/FacebookOAuth.aspx';

}]);