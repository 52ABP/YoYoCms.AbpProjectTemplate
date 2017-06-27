<!--修改用户信息-->
<style rel="styleesheet" lang="scss">
    .dialog--edit-user {
        .el-dialog {
            top: 15px !important;
        }

        .el-form-item {
            margin-bottom: 10px;
            &.is-error {
                margin-bottom: 22px;
            }
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

        <el-tabs v-model="activeName" v-loading="loading">
            <el-tab-pane label="用户管理" name="first">
                <el-form :model="currUser" :rules="rules" ref="form" label-width="170px" class="demo-ruleForm">
                    <el-form-item label="用户名" prop="userName">
                        <el-input v-model="currUser.userName" placeholder="用户名" :disabled="true"></el-input>
                    </el-form-item>
                    <el-form-item label="名字" prop="name">
                        <el-input v-model="currUser.name" placeholder="名字"></el-input>
                    </el-form-item>
                    <el-form-item label="姓氏" prop="surname">
                        <el-input v-model="currUser.surname" placeholder="姓氏"></el-input>
                    </el-form-item>
                    <el-form-item label="邮箱地址" prop="emailAddress">
                        <el-input v-model="currUser.emailAddress" placeholder="邮箱地址"></el-input>
                    </el-form-item>
                    <el-form-item label="手机号" prop="phoneNumber">
                        <el-input v-model="currUser.phoneNumber" placeholder="手机号码"></el-input>
                    </el-form-item>
                    <el-form-item label="使用随机密码" prop="setRandomPassword">
                        <el-checkbox v-model="fetchParam.setRandomPassword"></el-checkbox>
                    </el-form-item>
                    <el-form-item label="密码" prop="password" v-show="!fetchParam.setRandomPassword">
                        <el-input type="password" v-model="currUser.password" placeholder="密码"></el-input>
                    </el-form-item>
                    <el-form-item label="密码(核对)" prop="repassword" v-show="!fetchParam.setRandomPassword">
                        <el-input type="password" v-model="currUser.repassword" placeholder="密码(核对)"></el-input>
                    </el-form-item>
                    <el-form-item label="下次登录需要修改密码" prop="shouldChangePasswordOnNextLogin">
                        <el-checkbox v-model="currUser.shouldChangePasswordOnNextLogin"></el-checkbox>
                    </el-form-item>
                    <el-form-item label="发送激活邮件" prop="sendActivationEmail">
                        <el-checkbox v-model="fetchParam.sendActivationEmail"></el-checkbox>
                    </el-form-item>
                    <el-form-item label="激活" prop="isActive">
                        <el-checkbox v-model="currUser.isActive"></el-checkbox>
                    </el-form-item>
                    <el-form-item label="锁定" prop="isLockoutEnabled">
                        <el-checkbox v-model="currUser.isLockoutEnabled"></el-checkbox>
                    </el-form-item>
                </el-form>
            </el-tab-pane>
            <!--角色管理-->
            <el-tab-pane name="second">
                <span slot="label">
                    <el-badge :hidden="false" :value="assignedRoleCount" class="item">角色</el-badge>
                </span>

                <el-checkbox @change="setRoleCount" v-for="item in roles" v-model="item.isAssigned" :key="item.roleId">
                    {{item.roleDisplayName}}
                </el-checkbox>
            </el-tab-pane>
        </el-tabs>
        <span slot="footer" class="dialog-footer">
                <el-button @click="dialogVisible = false">取 消</el-button>
                <el-button type="primary" @click="confirmClick">保 存</el-button>
          </span>
    </el-dialog>
</template>

<script>
    import userService from '../../../services/userService'
    export default {
        props: {
            visiable: Boolean,
            user: Object
        },
        data() {
            // 验证密码核对
            let validRepass = (rule, value, callback) => {
                if (value !== this.currUser.password) {
                    callback(new Error('两次输入密码不一致!'))
                } else {
                    callback()
                }
            }
            return {
                loading: false,
                dialogVisible: this.visiable,
                title: '',
                activeName: 'first',
                currUser: this.user || {},
                rules: {
                    name: [{required: true, message: '请输入名字', trigger: 'change'}],
                    surname: [{required: true, message: '请输入姓氏', trigger: 'change'}],
                    emailAddress: [{required: true, message: '请输入邮箱', trigger: 'change'}],
                    repassword: [{validator: validRepass, trigger: 'change'}],
                },
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
        },
        methods: {
            // 确认按钮点击 提交数据
            async confirmClick() {
                this.loading = true
                let assignedRoleNames = []
                this.roles.forEach((item) => {
                    if (item.check) assignedRoleNames.push(item.name)
                })
                try {
                    await userService.createOrUpdateUser({
                        user: this.currUser,
                        ...this.fetchParam,
                        assignedRoleNames
                    })
                    this.loading = false
                    this.dialogVisible = false
                    abp.notify.success('修改成功!', '恭喜')

                    this.$emit('update:user', this.currUser)
                } catch (e) {
                    this.loading = false
                }
            },
            // 当弹框出现时调用
            async handleOpen () {
                this.activeName = 'first'
                this.loading = true
                this.title = '修改用户: ' + this.user.name

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
