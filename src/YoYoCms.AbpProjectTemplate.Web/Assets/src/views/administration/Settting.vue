<!--设置-->
<style rel="styleesheet" lang="scss">
    @import "../../mixins/topSearch";
    @import "../../mixins/common";

    .administration-settting-container {
        @extend %content-container;

        .tab-general {
            > i {
                font-size: 16px;
                color: #666;
            }
        }

        .el-form {
            h2 {
                font-size: 18px;
                color: #666;
            }

            i.tip {
                color: #999;
                margin-left: 8px;
            }
        }
    }
</style>

<template>
    <article class="administration-settting-container" v-loading="loading">
        <section class="right-top-btnContainer">
            <el-button type="primary" icon="save" @click="saveAll"><i
                    class="material-icons material-icons-small">save</i> 保存全部
            </el-button>
        </section>

        <el-tabs v-model="activeName" @tab-click="handleTabClick">
            <el-tab-pane label="基本信息" name="general">
                <i>时区</i>
                <Timezone v-model="general.timezone"></Timezone>
            </el-tab-pane>
            <el-tab-pane label="用户管理" name="userManagement" class="tab-general">
                <el-form :model="userManagement" label-position="left" ref="formUser"
                         label-width="260px">
                    <el-form label-width="0">
                        <h2>[基于表单身份验证]</h2>
                    </el-form>
                    <el-form-item label="允许用户注册" prop="name">
                        <el-checkbox v-model="userManagement.allowSelfRegistration"></el-checkbox>
                        <i class="tip">如果此项被禁用，只能由管理员通过用户管理页面添加用户.</i>
                    </el-form-item>
                    <el-form-item label="注册用户默认激活" prop="isNewRegisteredUserActiveByDefault">
                        <el-checkbox v-model="userManagement.isNewRegisteredUserActiveByDefault"></el-checkbox>
                        <i class="tip">如果此项被禁用，新用户需要通过邮件激活后才能登录.</i>
                    </el-form-item>
                    <el-form-item label="用户注册时使用图片验证码(captcha)." prop="useCaptchaOnRegistration">
                        <el-checkbox v-model="userManagement.useCaptchaOnRegistration"></el-checkbox>
                    </el-form-item>
                    <el-form label-width="0">
                        <h2>[其他设置]</h2>
                    </el-form>
                    <el-form-item label="必须验证邮箱地址后才能登录." prop="isEmailConfirmationRequiredForLogin">
                        <el-checkbox v-model="userManagement.isEmailConfirmationRequiredForLogin"></el-checkbox>
                    </el-form-item>
                </el-form>
            </el-tab-pane>
            <el-tab-pane label="保密信息" name="security">
                <el-form :model="security" label-position="left" :rules="securityRules" ref="formSecurity"
                         label-width="160px">
                    <el-form label-width="0">
                        <h2>[锁定用户]</h2>
                    </el-form>
                    <el-form-item label="启用登录失败锁定" prop="userLockOut.isEnabled">
                        <el-checkbox v-model="security.userLockOut.isEnabled"></el-checkbox>
                    </el-form-item>
                    <el-form-item v-show="security.userLockOut.isEnabled" label="最大登录失败次数"
                                  prop="userLockOut.maxFailedAccessAttemptsBeforeLockout">
                        <el-checkbox v-model="security.userLockOut.maxFailedAccessAttemptsBeforeLockout"></el-checkbox>
                    </el-form-item>
                    <el-form-item v-show="security.userLockOut.isEnabled" label="默认锁定时间"
                                  prop="userLockOut.defaultAccountLockoutSeconds">
                        <el-checkbox v-model="security.userLockOut.defaultAccountLockoutSeconds"></el-checkbox>
                    </el-form-item>
                </el-form>
            </el-tab-pane>
            <el-tab-pane label="邮箱(SMTP)" name="email">
                <el-form :model="email" label-position="left" :rules="emailRules" ref="formEmail" label-width="160px">
                    <el-form-item label="默认邮箱发送地址" prop="defaultFromAddress">
                        <el-input v-model="email.defaultFromAddress"></el-input>
                    </el-form-item>
                    <el-form-item label="默认发送人名字" prop="defaultFromDisplayName">
                        <el-input v-model="email.defaultFromDisplayName"></el-input>
                    </el-form-item>
                    <el-form-item label="SMTP服务器" prop="smtpHost">
                        <el-input v-model="email.smtpHost"></el-input>
                    </el-form-item>
                    <el-form-item label="SMTP端口" prop="smtpPort">
                        <el-input v-model="email.smtpPort"></el-input>
                    </el-form-item>
                    <el-form-item label="使用SSL" prop="smtpEnableSsl">
                        <el-checkbox v-model="email.smtpEnableSsl"></el-checkbox>
                    </el-form-item>
                    <el-form-item label="默认身份验证" prop="smtpUseDefaultCredentials">
                        <el-checkbox v-model="email.smtpUseDefaultCredentials"></el-checkbox>
                    </el-form-item>
                    <el-form-item v-show="!email.smtpUseDefaultCredentials" label="域名" prop="smtpDomain">
                        <el-input v-model="email.smtpDomain"></el-input>
                    </el-form-item>
                    <el-form-item v-show="!email.smtpUseDefaultCredentials" label="用户名" prop="smtpUserName">
                        <el-input v-model="email.smtpUserName"></el-input>
                    </el-form-item>
                    <el-form-item v-show="!email.smtpUseDefaultCredentials" label="密码" prop="smtpPassword">
                        <el-input type="password" v-model="email.smtpPassword"></el-input>
                    </el-form-item>
                </el-form>
            </el-tab-pane>
        </el-tabs>
    </article>
</template>

<script>
    import Timezone from '../../components/select/TimeZone.vue'
    import tenantSettingsService from '../../services/administration/tenantSettingsService'
    export default {
        data() {
            return {
                loading: false,
                activeName: 'general',
                general: {},
                userManagement: {},
                security: {userLockOut: {}},
                email: {},

                userRules: {},
                securityRules: {},
                emailRules: {}
            }
        },
        created() {
        },
        activated() {
            this.fetchData()
        },
        methods: {
            async fetchData () {
                this.loading = true
                let ret = await tenantSettingsService.getAllSettings()
                this.general = ret.general
                this.userManagement = ret.userManagement
                this.security = ret.security
                this.email = ret.email

                this.loading = false
            },
            handleTabClick(tab) {
                console.log(tab)
            },
            async saveAll() {
                try {
                    this.loading = true
                    await tenantSettingsService.updateAllSettings({
                        email: this.email,
                        security: this.security,
                        userManagement: this.userManagement,
                        general: this.general
                    })
                    abp.notify.success('保存成功!', '恭喜')
                } finally {
                    this.loading = false
                }
            }
        },
        components: {
            Timezone
        }
    }
</script>
