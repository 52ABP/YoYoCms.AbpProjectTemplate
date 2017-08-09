<!--图片裁切上传-->
<!--<ImgCropper :isShowBtn="false" ref="imgcropper" :confirmFn="cropperImgSucc" :aspectRatio="1"-->
<!--:isRound="true"></ImgCropper>-->
<style lang='scss' scoped rel='stylesheet/scss'>
    .croppercontainer {
        img {
            max-height: 600px;
        }
    }
</style>

<template>
    <article ref="container">
        <el-button v-show="isShowBtn" type="primary" @click="chooseImg">{{L('Upload')}}<i class="el-icon-upload el-icon--right"></i>
        </el-button>
        <el-dialog :close-on-click-modal="false" :title="L('CropImage')" v-model="showCropper" size="small" top="15px">
            <div class="croppercontainer">
                <img @load="startCropper()" class="image-preview" :src="imgData">
            </div>
            <span slot="footer">
                <el-button @click="showCropper = false">{{L('Cancel')}}</el-button>
                <el-button type="primary" @click="confirmCropper()">{{L('Save')}}</el-button>
            </span>
        </el-dialog>

        <input accept="image/jpg,image/jpeg,image/png" type="file" style="display: none" @change="fileChange($event)"
               ref="file">
    </article>
</template>

<script>
    import Cropper from 'cropperjs'
    import '../../../node_modules/cropperjs/dist/cropper.min.css'

    export default{
        props: {
            confirmFn: Function, // 点击确认后的回调 参数: imgData-base64的数据
            aspectRatio: { // 长宽比例 16/9
                type: Number,
                default: 16 / 9
            },
            isRound: { // 是否圆形裁切
                type: Boolean,
                default: false
            },
            isShowBtn: { // 是否显示上传按钮
                isShowBtn: Boolean,
                default: true
            },
            compress: { //  压缩比例
                type: Number,
                default: 0.8
            },
        },
        data () {
            return {
                showCropper: false,
                imgData: '',
                finalRatio: null,
                cropper: null,
                imageType: void 0,
                ext: void 0,
            }
        },
        watch: {
            'showCropper'(val) {
                if (!val) {
                    this.cropper.destroy()
                    this.imgData = null
                }
            }
        },
        created () {
            this.finalRatio = this.aspectRatio || 16 / 9
        },
        methods: {
            // 选择图片 向外部暴露的接口
            chooseImg () {
                this.$refs.file.click()
            },
            // 文件切换选中
            fileChange (e) {
                let file = e.target.files[0]
                if (!file)
                    return

                let _this = this
                let reader = new window.FileReader()
                reader.readAsDataURL(file)
                reader.onload = function () {
                    var ext = 'jpg'
                    var exts = file.name.split('.')
                    if (exts.length > 0 && (exts.length - 1) > 0) {
                        ext = exts[(exts.length - 1)]
                    }
                    _this.$nextTick(() => {
                        _this.imgData = this.result
                        _this.$refs.file.value = null
                        _this.showCropper = true
                        _this.imageType = file.type
                        _this.ext = '.' + ext
                    })
                }
            },
            // 确定裁剪尺寸
            confirmCropper () {
                this.showCropper = false
                let retBase64 = this.cropper.getCroppedCanvas().toDataURL(`image/${this.imageType}`, this.compress)
                this.confirmFn && this.confirmFn(retBase64, this.ext, retBase64.split(';')[1].substr(7))
                this.imgData = null
            },
            startCropper () {
                let image = this.$refs.container.querySelector('.image-preview')
                let _this = this
                this.cropper = new Cropper(image, {
                    aspectRatio: this.finalRatio,
                    scalable: false,
                    zoomable: false,
                    viewMode: 1,
                    ready() {
                        // 是否需要原型裁切
                        if (_this.isRound) {
                            [..._this.$refs.container.querySelectorAll('.cropper-view-box,.cropper-face')].map(item => {
                                item.style['border-radius'] = '50%'
                            })
                        }
                    }
                })
            }
        },
        components: {}
    }
</script>
