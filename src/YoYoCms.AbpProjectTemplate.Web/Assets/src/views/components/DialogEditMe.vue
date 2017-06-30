<!--首页修改用户信息的弹出框-->
<style rel="styleesheet" lang="scss">
</style>

<template>
    <el-dialog
            title="我的信息"
            :visible.sync="dialogVisible"
            size="tiny">

        <span slot="footer" class="dialog-footer">
            <el-button @click="dialogVisible = false">取 消</el-button>
            <el-button type="primary" @click="dialogVisible = false">确 定</el-button>
          </span>
    </el-dialog>
</template>

<script>
    export default {
        props: {
            user: this.$store.state.auth.user
        },
        data() {
            return {}
        },
        watch: {
            'visible' (val) {
                if (val != this.dialogVisible) this.dialogVisible = val
            },
            'dialogVisible' (val) {
                this.$emit('update:visible', val)
            }
        },
        created() {
        },
        activated() {
        },
        methods: {},
        components: {}
    }
</script>
