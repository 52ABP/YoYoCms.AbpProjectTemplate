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

        // 未激活提示
        .not-active {
            ul {
                /*margin-left: 35px;*/
                em {
                    margin-right: 2px;
                    font-weight: bold;
                }
            }
            p {
                border-radius: 5px;
                background-color: #f9e491;
                border-color: #f9e491;
                color: #c29d0b;
                box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1), 0 1px 2px rgba(0, 0, 0, 0.18);
                line-height: 1.8em;
                padding: 8px;
                margin-top: 15px;
            }
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
                    <div class="body" v-loading="loading" v-if="!registerSucc">
                        <div class="msg">{{L('CreateAnAccount')}}</div>
                        <el-form ref="form" :model="fetchParam" label-position="center" :rules="rules" label-width="7em"
                                 class="demo-ruleForm">
                            <el-form-item label-width="0">
                                <h2>{{L('PersonalInformations')}}</h2>
                            </el-form-item>
                            <el-form-item :label="L('Name')" prop="name">
                                <input type="text" ref="txtName" class="form-control" name="username"
                                       :placeholder="L('Name')" v-model="fetchParam.name" autofocus>
                            </el-form-item>
                            <el-form-item prop="surname" :label="L('Surname')">
                                <input type="text" @keyup.enter="submit" v-model="fetchParam.surname"
                                       class="form-control" :placeholder="L('Surname')">
                            </el-form-item>
                            <el-form-item prop="emailAddress" :label="L('EmailAddress')">
                                <input type="email" @keyup.enter="submit" v-model="fetchParam.emailAddress"
                                       class="form-control" :placeholder="L('EmailAddress')">
                            </el-form-item>
                            <el-form-item label-width="0">
                                <h2>{{L('AccountSettings')}}</h2>
                            </el-form-item>
                            <el-form-item prop="userName" :label="L('UserName')">
                                <input type="text" @keyup.enter="submit" v-model="fetchParam.userName"
                                       class="form-control" :placeholder="L('UserName')"
                                       required>
                            </el-form-item>
                            <el-form-item prop="password" :label="L('Password')">
                                <input type="password" @keyup.enter="submit" v-model="fetchParam.password"
                                       class="form-control" :placeholder="L('Password')"
                                       required>
                            </el-form-item>
                            <el-form-item prop="passwordRepeat" :label="L('PasswordRepeat')">
                                <input type="password" @keyup.enter="submit" v-model="fetchParam.passwordRepeat"
                                       class="form-control" :placeholder="L('PasswordRepeat')"
                                       required>
                            </el-form-item>
                        </el-form>
                        <div class="row">
                            <div class="col-xs-8 p-t-5">
                            </div>
                            <div class="col-xs-4">
                                <button @click="submit" class="btn btn-block bg-pink waves-effect" type="submit">
                                    {{L('Submit')}}
                                </button>
                            </div>
                        </div>
                    </div>
                    <div class="body not-active" v-else>
                        <h2>{{L('SuccessfullyRegistered')}}</h2>
                        <ul>
                            <li>
                                <em>{{L('Name')}}: </em> <i>{{fetchParam.name}}</i>
                            </li>
                            <li>
                                <em>{{L('TenancyName')}}: </em> <i>Default</i>
                            </li>
                            <li>
                                <em>{{L('UserName')}}: </em> <i>{{fetchParam.userName}}</i>
                            </li>
                            <li>
                                <em>{{L('EmailAddress')}}: </em> <i>{{fetchParam.emailAddress}}</i>
                            </li>
                        </ul>

                        <p>
                            {{L('YourAccountIsWaitingToBeActivatedByAdmin')}}
                        </p>
                    </div>
                    <div class="row">
                        <div class="col-xs-5 col-xs-offset-1" style="margin-bottom: 10px">
                            <Languages></Languages>
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
    import Languages from './components/Languages.vue'
    //    import authUtils from '../../common/utils/authUtils'
    export default {
        data() {
            let validRepass = (rule, value, callback) => {
                if (value != this.fetchParam.password)
                    callback(new Error(lang.L('RepeatPasswordError')))
                else
                    callback()
            }
            return {
                loading: false,
                fetchParam: {
                    name: '',
                    surname: '',
                    emailAddress: '',
                    userName: '',
                    password: '',
                    passwordRepeat: '',
                },
                rules: {
                    name: [{type: 'string', required: true, message: lang.L('RequiredFiled'), trigger: 'change'}],
                    surname: [{type: 'string', required: true, message: lang.L('RequiredFiled'), trigger: 'change'}],
//                    password: [{type: 'string', required: true, message: '请输入新密码', trigger: 'blur'}],
                    emailAddress: [{type: 'string', required: true, message: lang.L('RequiredFiled'), trigger: 'blur'}],
                    userName: [{type: 'string', required: true, message: lang.L('RequiredFiled'), trigger: 'blur'}],
                    password: [{type: 'string', required: true, message: lang.L('RequiredFiled'), trigger: 'blur'}],
                    passwordRepeat: [{validator: this.validRepass, trigger: 'change'}],
                },
                registerSucc: false,
                validRepass
            }
        },
        mounted() {
            this.$refs.txtName.focus()
        },
        methods: {
            submit () {
                this.$refs.form.validate(async (valid) => {
                    if (!valid) return
                    this.loading = true

                    try {
                        let ret = await userService.register(this.fetchParam)
    
                        setTimeout(() => {
//                        this.$router.push({name: 'Dashboard.Tenant'})
                            abp.notify.success(lang.L('SuccessfullyRegistered'), lang.L('Success'))
                            this.loading = false
                            if (!ret.isActive) this.registerSucc = true
                            else this.$router.push({name: 'Dashboard.Tenant'})
                        }, 5e2)
                    } catch (e) {
                        this.loading = false
                    }
                })
            },
        },
        components: {Languages}
    }
</script>