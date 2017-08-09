<!--修改头像弹出框-->
<style rel="styleesheet" lang="scss">
</style>

<template>
    <article>
        <el-dialog
                :title="L('ChangeProfilePicture')"
                :visible.sync="dialogVisible"
                size="tiny">

            <div>
                <el-button @click="choose">{{L('ChooseImg')}}</el-button>
                {{L('ProfilePicture_Change_Info')}}
            </div>
            <span slot="footer" class="dialog-footer">
            <el-button @click="dialogVisible = false">{{L('Cancel')}}</el-button>
          </span>
        </el-dialog>

        <CropperImg ref="copper" :aspectRatio="1" :isShowBtn="false" :isRound="true"
                    :confirmFn="cropperCb"></CropperImg>
    </article>
</template>

<script>
    import profileService from '../../services/administration/profileService'
    import CropperImg from '../../components/upload/ImgEcropper.vue'
    export default {
        props: {
            visible: Boolean,
        },
        data() {
            return {
                dialogVisible: false
            }
        },
        watch: {
            'visible' (val) {
                if (val != this.dialogVisible) this.dialogVisible = val
            },
            'dialogVisible' (val) {
                this.$emit('update:visible', val)
            },
        },
        created() {
        },
        activated() {
        },
        methods: {
            // 选择图片
            choose() {
                this.$refs.copper.chooseImg()
            },
            async cropperCb (data, ext, retBase64) {
                await profileService.uploadPortrait({
                    imgData: retBase64,
                    fileName: Date.now() + ext
                })
                this.dialogVisible = false
                this.$store.dispatch('setAuthUser', {user: Object.assign({}, this.$store.state.auth.user, {portrait: data})})
                abp.notify.success(lang.L('SavedSuccessfully'), lang.L('Success'))
            }
        },
        components: {CropperImg}
    }
</script>
