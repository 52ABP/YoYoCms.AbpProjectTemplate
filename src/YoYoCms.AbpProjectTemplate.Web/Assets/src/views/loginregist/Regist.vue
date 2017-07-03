<!--注册-->
<style rel="styleesheet" lang="scss">
    .register-container {
        background: #00BCD4;
        position: absolute;
        width: 100%;
        height: 100%;
        .el-form-item__content {
            line-height: 0
        }

        h2 {
            font-size: 18px;
            margin-top: 0;
        }
    }
</style>
<template>
    <article class="register-container">
        <article class="login-page">
            <div class="login-box">
                <div class="logo">
                    <a href="javascript:void(0);">Abp<b>ProjectTemplate</b></a>
                    <small>Admin BootStrap Based - Material Design</small>
                </div>
                <div class="card">
                    <div class="body" v-loading="loading">
                        <div class="msg">注册</div>
                        <el-form ref="form" :model="fetchParam" label-position="center" :rules="rules" label-width="7em"
                                 class="demo-ruleForm">
                            <el-form-item label-width="0">
                                <h2>个人信息</h2>
                            </el-form-item>
                            <el-form-item label="名字" prop="name">
                                <input type="text" ref="txtName" class="form-control" name="username"
                                       placeholder="请输入名字" v-model="fetchParam.name" autofocus>
                            </el-form-item>
                            <el-form-item prop="surname" label="姓氏">
                                <input type="text" @keyup.enter="submit" v-model="fetchParam.surname"
                                       class="form-control" placeholder="请输入姓氏">
                            </el-form-item>
                            <el-form-item prop="emailAddress" label="邮箱地址">
                                <input type="email" @keyup.enter="submit" v-model="fetchParam.emailAddress"
                                       class="form-control" placeholder="请输入邮箱地址">
                            </el-form-item>
                            <el-form-item label-width="0">
                                <h2>账号设置</h2>
                            </el-form-item>
                            <el-form-item prop="userName" label="用户名">
                                <input type="text" @keyup.enter="submit" v-model="fetchParam.userName"
                                       class="form-control" placeholder="请输入用户名"
                                       required>
                            </el-form-item>
                            <el-form-item prop="password" label="密码">
                                <input type="password" @keyup.enter="submit" v-model="fetchParam.password"
                                       class="form-control" placeholder="请输入密码"
                                       required>
                            </el-form-item>
                            <el-form-item prop="passwordRepeat" label="重复密码">
                                <input type="password" @keyup.enter="submit" v-model="fetchParam.passwordRepeat"
                                       class="form-control" placeholder="确认密码(核对)"
                                       required>
                            </el-form-item>
                        </el-form>
                        <div class="row">
                            <div class="col-xs-8 p-t-5">
                                <!--<input type="checkbox" name="rememberme" id="rememberme"-->
                                <!--class="filled-in chk-col-pink">-->
                                <!--<label for="rememberme">记住我</label>-->
                            </div>
                            <div class="col-xs-4">
                                <button @click="submit" class="btn btn-block bg-pink waves-effect" type="submit">提交
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </article>
    </article>
</template>

<script>
    //    import config from '../../common/config'
    import userService from '../../services/userService'
    //    import authUtils from '../../common/utils/authUtils'
    export default {
        data() {
            let validRepass = (rule, value, callback) => {
                if (value != this.fetchParam.password)
                    callback(new Error('两次输入密码不一致!'))
                else
                    callback()
            }
            return {
                loading: false,
                fetchParam: {
                    name: void 0,
                    surname: void 0,
                    emailAddress: void 0,
                    userName: void 0,
                    password: '',
                    passwordRepeat: '',
                },
                rules: {
                    name: [{type: 'string', required: true, message: '请输入名字', trigger: 'change'}],
                    surname: [{type: 'string', required: true, message: '请输入姓氏', trigger: 'change'}],
//                    password: [{type: 'string', required: true, message: '请输入新密码', trigger: 'blur'}],
                    emailAddress: [{type: 'string', required: true, message: '请输入邮箱地址', trigger: 'blur'}],
                    userName: [{type: 'string', required: true, message: '请输入用户名', trigger: 'blur'}],
                    password: [{type: 'string', required: true, message: '请输入密码', trigger: 'blur'}],
                    passwordRepeat: [{validator: this.validRepass, trigger: 'change'}],
                },
                validRepass
            }
        },
        mounted() {
            this.$refs.txtName.focus()
        },
        methods: {
            submit () {
                this.$refs.form.validate((valid) => {
                    if (!valid) return
                    this.loading = true
                    let params = Object.assign({}, this.$route.query, this.fetchParam)

                    userService.resetPwd(params)
                    setTimeout(() => {
//                        authUtils.setToken(ret)
                        this.$router.push({name: 'Dashboard.Tenant'})
                        abp.notify.success('登录成功!', '恭喜')
                        this.loading = false
                    }, 5e2)
                })
            },
        },
        components: {}
    }
</script>