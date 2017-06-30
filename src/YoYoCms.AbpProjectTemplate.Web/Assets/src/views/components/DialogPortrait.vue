<!--修改头像弹出框-->
<style rel="styleesheet" lang="scss">
</style>

<template>
    <article>
        <el-dialog
                title="头像修改"
                :visible.sync="dialogVisible"
                size="tiny">

            <div>
                <el-button @click="choose">选择图片</el-button>
                只能选择1mb内的JPG/JPEG/PNG图片.
            </div>
            <span slot="footer" class="dialog-footer">
            <el-button @click="dialogVisible = false">取 消</el-button>
          </span>
        </el-dialog>

        <CropperImg ref="copper" :aspectRatio="1" :isShowBtn="false" :isRound="true"
                    :confirmFn="cropperCb"></CropperImg>
    </article>
</template>

<script>
    import profileService from '../../services/profileService'
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
                abp.notify.success('修改成功', '恭喜')
            }
        },
        components: {CropperImg}
    }
</script>
