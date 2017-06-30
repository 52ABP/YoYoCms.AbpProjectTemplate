<!--首页修改用户信息的弹出框-->
<style rel="styleesheet" lang="scss">
</style>

<template>
    <el-dialog
            title="我的信息"
            :visible.sync="dialogVisible"
            size="tiny">
        <el-form :model="fetchParam" :rules="rules" ref="ruleForm" label-width="100px" class="demo-ruleForm">
            <el-form-item label="活动名称" prop="name">
                <el-input v-model="ruleForm.name"></el-input>
            </el-form-item>
        </el-form>
        <span slot="footer" class="dialog-footer">
            <el-button @click="dialogVisible = false">取 消</el-button>
            <el-button type="primary" @click="dialogVisible = false">保 存</el-button>
          </span>
    </el-dialog>
</template>

<script>
    export default {
        props: {
            visible: Boolean
        },
        data() {
            return {
                user: this.$store.state.auth.user,
                dialogVisible: false,
                rules: {},
                fetchParam: {}
            }
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
            console.log(this.user)
        },
        activated() {
        },
        methods: {},
        components: {}
    }
</script>
