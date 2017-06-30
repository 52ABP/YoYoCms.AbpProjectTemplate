<!--首页修改个人信息的弹出框-->
<style rel="styleesheet" lang="scss">
</style>

<template>
    <el-dialog
            title="我的信息"
            :visible.sync="dialogVisible"
            size="tiny">
        <el-form :model="user" :rules="rules" ref="form" label-width="100px">
            <el-form-item label="用户名" prop="userName">
                <el-input v-model="user.userName" placeholder="用户名" :disabled="true"></el-input>
            </el-form-item>
            <el-form-item label="名字" prop="name">
                <el-input v-model="user.name"></el-input>
            </el-form-item>
            <el-form-item label="姓氏" prop="surname">
                <el-input v-model="user.surname"></el-input>
            </el-form-item>
            <el-form-item label="邮箱地址" prop="emailAddress">
                <el-input v-model="user.emailAddress"></el-input>
            </el-form-item>
            <el-form-item label="手机号" prop="phoneNumber">
                <el-input v-model="user.phoneNumber" placeholder="手机号"></el-input>
            </el-form-item>
        </el-form>
        <span slot="footer" class="dialog-footer">
            <el-button @click="dialogVisible = false">取 消</el-button>
            <el-button type="primary" @click="save">
                <i class="material-icons" style="font-size: 14px;vertical-align: middle">save</i> 保 存</el-button>
          </span>
    </el-dialog>
</template>

<script>
    import profileService from '../../services/profileService'
    import clone from 'clone'
    export default {
        props: {
            visible: Boolean,
        },
        data() {
            return {
                user: {},
                dialogVisible: false,
                rules: {
                    name: [{required: true, message: '请输入名字', trigger: 'change'}],
                    surname: [{required: true, message: '请输入姓氏', trigger: 'change'}],
                    emailAddress: [{required: true, message: '请输入邮箱', trigger: 'change'}]
                },
            }
        },
        watch: {
            'visible' (val) {
                if (val != this.dialogVisible) this.dialogVisible = val
            },
            'dialogVisible' (val) {
                if (val) this.user = clone(this.$store.state.auth.user)
                this.$emit('update:visible', val)
            },
        },
        created() {
        },
        activated() {
        },
        methods: {
            save() {
                this.$refs.form.validate(async (valid) => {
                    if (!valid) return
                    try {
                        await profileService.updateCurrentUserProfile(this.user)

                        this.$store.dispatch('setAuthUser', {user: this.user})
                        this.dialogVisible = false
                    } finally {
                    }
                })
            }
        },
        components: {}
    }
</script>
