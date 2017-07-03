<!--用户列表弹窗-->
<style rel="styleesheet" lang="scss">
</style>

<template>
    <article class="dialog-permission-tree--container" v-loading="loading">
        <el-dialog ref="dialog"
                   @open="handleOpen"
                   :title="title"
                   :visible.sync="dialogVisible"
                   size="tiny">

            <span slot="footer" class="dialog-footer">
                <el-button @click="dialogVisible = false">取 消</el-button>
                <el-button type="primary" @click="confirmClick">确 定</el-button>
          </span>
        </el-dialog>
    </article>
</template>

<script>
    import userService from '../../services/userService'
    export default {
        props: {
            title: String,
            value: Boolean,
            userid: Number,
            onConfirmCb: Function, // 确定按钮的回调 参数为 选中的权限id集合
        },
        data() {
            return {
                dialogVisible: this.value,
                loading: false,
                defaultProps: {
                    children: 'children',
                    label: 'label'
                },
//                user: this.$store.state.auth.user,
                orignPermission: void 0,
                treeData: [],
                plugins: ['checkbox', 'types'], // jstree的plugins参数
            }
        },
        watch: {
            'value' (val) {
                if (val != this.dialogVisible) this.dialogVisible = val
            },
            'dialogVisible' (val) {
                this.$emit('input', val)
            }
        },
        async activated() {
        },
        async mounted () {
        },
        methods: {
            fetchData () {
            },
            async handleOpen () {
                this.loading = true
                this.orignPermission = await userService.getUserPermissionsForEdit({id: this.userid})
                this.loading = false
            },
            // 确定按钮点击回调
            confirmClick () {
                if (!this.onConfirmCb) this.dialogVisible = false

                this.loading = true
                this.onConfirmCb(this.$refs.dialog.$children[2].get_selected()).then(() => {
                    this.dialogVisible = false
                    this.loading = false
                })
            }
        },
        components: {}
    }
</script>

