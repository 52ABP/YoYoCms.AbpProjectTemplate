(function($) {
    app.modals.ChangeProfilePictureModal = function () {

        var _modalManager;
        var $jcropImage = null;
        var uploadedFileName = null;

        var _profileService = abp.services.app.profile;

        this.init = function(modalManager) {
            _modalManager = modalManager;

            $('#ChangeProfilePictureModalForm input[name=ProfilePicture]').change(function() {
                $('#ChangeProfilePictureModalForm').submit();
            });

            $('#ChangeProfilePictureModalForm').ajaxForm({
                beforeSubmit: function (formData, jqForm, options) {

                    var $fileInput = $('#ChangeProfilePictureModalForm input[name=ProfilePicture]');
                    var files = $fileInput.get()[0].files;

                    if (!files.length) {
                        return false;
                    }

                    var file = files[0];

                    //File type check
                    var type = '|' + file.type.slice(file.type.lastIndexOf('/') + 1) + '|';
                    if ('|jpg|jpeg|png|gif|'.indexOf(type) === -1) {
                        abp.message.warn(app.localize('ProfilePicture_Warn_FileType'));
                        return false;
                    }

                    //File size check
                    if (file.size > 1048576) //1MB
                    {
                        abp.message.warn(app.localize('ProfilePicture_Warn_SizeLimit'));
                        return false;
                    }

                    return true;
                },
                success: function (response) {
                    if (response.success) {
                        var $profilePictureResize = $('#ProfilePictureResize');

                        var newCanvasHeight = response.result.height * $profilePictureResize.width() / response.result.width;
                        $profilePictureResize.height(newCanvasHeight + 'px');

                        var profileFilePath = abp.appPath + 'Temp/Downloads/' + response.result.fileName + '?v=' + new Date().valueOf();
                        uploadedFileName = response.result.fileName;

                        if ($jcropImage) {
                            $jcropImage.data('Jcrop').destroy();
                        }

                        $profilePictureResize.show();
                        $profilePictureResize.attr('src', profileFilePath);
                        $jcropImage = $profilePictureResize.Jcrop({
                            trueSize: [response.result.width, response.result.height],
                            setSelect: [0, 0, 100, 100],
                            aspectRatio: 1
                        });

                    } else {
                        abp.message.error(response.error.message);
                    }
                }
            });

            $('#ProfilePictureResize').hide();
        };

        this.save = function () {
            if (!uploadedFileName) {
                return;
            }

            var resizeParams = {};
            if ($jcropImage) {
                resizeParams = $jcropImage.data("Jcrop").tellSelect();
            }

            _profileService.updateProfilePicture({
                fileName: uploadedFileName,
                x: parseInt(resizeParams.x),
                y: parseInt(resizeParams.y),
                width: parseInt(resizeParams.w),
                height: parseInt(resizeParams.h)
            }).done(function () {
                $jcropImage.data('Jcrop').destroy();
                $jcropImage = null;
                $('#HeaderProfilePicture').attr('src', app.getUserProfilePicturePath());
                _modalManager.close();
            });
        };
    };
})(jQuery);