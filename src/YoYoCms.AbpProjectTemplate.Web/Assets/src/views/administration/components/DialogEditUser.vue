<!--修改用户信息-->
<style rel="styleesheet" lang="scss">
    .dialog--edit-user {
        .el-dialog {
            top: 15px !important;
        }

        .el-badge__content.is-fixed {
            top: 7px;
        }
    }
</style>

<template>
    <el-dialog ref="dialog" class="dialog--edit-user"
               :title="title"
               @open="handleOpen"
               :visible.sync="dialogVisible"
               size="small">

        <el-tabs v-model="activeName" v-loading="loading" ref="tab">
            <el-tab-pane :label="L('UserInformations')" name="first">
                <el-form :model="currUser" :rules="rules" ref="form" label-width="170px" class="demo-ruleForm">
                    <el-form-item :label="L('UserName')" prop="userName">
                        <el-input v-model="currUser.userName" :placeholder="L('UserName')" :disabled="!!currUser.id"></el-input>
                    </el-form-item>
                    <el-form-item :label="L('Name')" prop="name">
                        <el-input v-model="currUser.name" :placeholder="L('Name')"></el-input>
                    </el-form-item>
                    <el-form-item :label="L('Surname')" prop="surname">
                        <el-input v-model="currUser.surname" :placeholder="L('Surname')"></el-input>
                    </el-form-item>
                    <el-form-item :label="L('EmailAddress')" prop="emailAddress">
                        <el-input v-model="currUser.emailAddress" :placeholder="L('EmailAddress')"></el-input>
                    </el-form-item>
                    <el-form-item :label="L('PhoneNumber')" prop="phoneNumber">
                        <el-input v-model="currUser.phoneNumber" :placeholder="L('PhoneNumber')"></el-input>
                    </el-form-item>
                    <el-form-item :label="L('SetRandomPassword')" prop="setRandomPassword">
                        <el-checkbox v-model="fetchParam.setRandomPassword"></el-checkbox>
                    </el-form-item>
                    <el-form-item :label="L('Password')" prop="password" v-show="!fetchParam.setRandomPassword">
                        <el-input type="password" v-model="currUser.password" :placeholder="L('Password')"></el-input>
                    </el-form-item>
                    <el-form-item :label="L('PasswordRepeat')" prop="repassword" v-show="!fetchParam.setRandomPassword">
                        <el-input type="password" v-model="currUser.repassword" :placeholder="L('PasswordRepeat')"></el-input>
                    </el-form-item>
                    <el-form-item :label="L('ShouldChangePasswordOnNextLogin')" prop="shouldChangePasswordOnNextLogin">
                        <el-checkbox v-model="currUser.shouldChangePasswordOnNextLogin"></el-checkbox>
                    </el-form-item>
                    <el-form-item :label="L('SendActivationEmail')" prop="sendActivationEmail">
                        <el-checkbox v-model="fetchParam.sendActivationEmail"></el-checkbox>
                    </el-form-item>
                    <el-form-item :label="L('Active')" prop="isActive">
                        <el-checkbox v-model="currUser.isActive"></el-checkbox>
                    </el-form-item>
                    <el-form-item :label="L('IsLockoutEnabled')" prop="isLockoutEnabled">
                        <el-checkbox v-model="currUser.isLockoutEnabled"></el-checkbox>
                    </el-form-item>
                </el-form>
            </el-tab-pane>
            <!--角色管理-->
            <el-tab-pane name="second">
                <span slot="label">
                    <el-badge :hidden="false" :value="assignedRoleCount" class="item">{{L('Roles')}}</el-badge>
                </span>

                <el-checkbox @change="setRoleCount" v-for="item in roles" v-model="item.isAssigned" :key="item.roleId">
                    {{item.roleDisplayName}}
                </el-checkbox>
            </el-tab-pane>
        </el-tabs>
        <span slot="footer" class="dialog-footer">
                <el-button @click="dialogVisible = false">{{L('Cancel')}}</el-button>
                <el-button type="primary" @click="confirmClick">{{L('Save')}}</el-button>
          </span>
    </el-dialog>
</template>

<script>
    import userService from '../../../services/userService'
    export default {
        props: {
            visiable: Boolean,
            user: Object,
            onSaved: Function, // 保存成功后回调
        },
        data() {
            // 验证密码核对
            let validRepass = (rule, value, callback) => {
                if (value != this.currUser.password)
                    callback(new Error('两次输入密码不一致!'))
                else
                    callback()
            }
            return {
                loading: false,
                dialogVisible: this.visiable,
                title: '',
                activeName: 'first',
                currUser: this.user || {},
                rules: {
                    name: [{required: false, message: '请输入名字', trigger: 'change'}],
                    surname: [{required: false, message: '请输入姓氏', trigger: 'change'}],
                    emailAddress: [{required: true, message: '请输入邮箱', trigger: 'change'}],
                    repassword: [{validator: this.validRepass, trigger: 'change'}],
                    userName: [{required: true, message: '请输入用户名', trigger: 'change'}],
                },
                validRepass,
                fetchParam: {
                    sendActivationEmail: false,
                    setRandomPassword: false, // 使用随机密码
                },
                roles: [], // 所有的角色
                assignedRoleCount: 0, // 拥有的角色数量
            }
        },
        watch: {
            'dialogVisible' (val) {
                this.$emit('update:visiable', val)
            },
            'visiable' (val) {
                if (val != this.dialogVisible) this.dialogVisible = val
            },
            'fetchParam.setRandomPassword' (val) {
                if (!val) {
                    this.rules['repassword'] = [{validator: this.validRepass, trigger: 'change'}]
                } else {
                    delete this.rules['repassword']
                }
            }
        },
        methods: {
            // 确认按钮点击 提交数据
            async confirmClick() {
                await new Promise((resolve, reject) => {
                    this.$refs.form.validate((valid) => {
                        valid ? resolve(true) : reject('表单验证失败' + valid)
                    })
                })
                this.loading = true
                this.currUser.roles = []
                let assignedRoleNames = []
                this.roles.forEach((item) => {
                    if (item.isAssigned) {
                        assignedRoleNames.push(item.roleName)
                        this.currUser.roles.push({roleName: item.roleName})
                    }
                })
                try {
                    await userService.createOrUpdateUser({
                        user: this.currUser,
                        ...this.fetchParam,
                        assignedRoleNames
                    })
                    this.loading = false
                    this.dialogVisible = false
                    abp.notify.success(lang.L('SavedSuccessfully'), lang.L('Success'))

                    this.$emit('update:user', this.currUser)

                    // 保存完毕回调
                    this.onSaved && this.onSaved(this.user.id ? 'edit' : 'add')
                } catch (e) {
                    this.loading = false
                }
            },
            // 当弹框出现时调用
            async handleOpen () {
                this.$nextTick(() => {
                    this.$refs.form.resetFields()
                })
                this.activeName = 'first'
                this.loading = true
                this.title = `${lang.L('EditUser')}: ${this.user.name}`
                this.rules['repassword'] = [{validator: this.validRepass, trigger: 'change'}]
                this.fetchParam.setRandomPassword = this.fetchParam.sendActivationEmail = false
                if (!this.user.id) {
                    this.title = lang.L('CreateNewUser')
                    this.fetchParam.setRandomPassword = this.fetchParam.sendActivationEmail = true
                    delete this.rules['repassword']
                }

                // 获取用户信息
                let retUser = await userService.getUserForEdit({id: this.user.id})
                this.currUser = retUser.user
                this.roles = retUser.roles

                this.loading = false
                this.setRoleCount() // 设置拥有的角色数量
            },
            // 设置拥有的角色数量
            setRoleCount () {
                this.assignedRoleCount = 0
                this.roles.forEach((item) => {
                    if (item.isAssigned) this.assignedRoleCount++
                })
            }
        },
        components: {}
    }
</script>
