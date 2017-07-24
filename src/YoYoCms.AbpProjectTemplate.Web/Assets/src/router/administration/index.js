/**
 * Created by huanghx on 2017/6/25.
 */
const namePre = 'Administration.'
export default {
    path: '/administration',
    name: 'Administration',
    component: resolve => {
        require.ensure([], () => {
            resolve(require('../../views/administration/Index.vue'))
        })
    },
    children: [{ // 组织结构
        path: 'organizationunits',
        name: namePre + 'OrganizationUnits',
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/OrganizationUnits.vue'))
            })
        },
        meta: {
            permission: 'Pages.Administration.OrganizationUnits'
        }
    }, { // 角色信息
        path: 'roles',
        name: namePre + `Roles`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/Roles.vue'))
            })
        },
        meta: {
            permission: 'Pages.Administration.Roles'
        }
    }, { // 用户信息
        path: 'users',
        name: namePre + `Users`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/Users.vue'))
            })
        },
        meta: {
            permission: 'Pages.Administration.Users'
        }
    }, { // 语言列表
        path: 'languages',
        name: namePre + `Languages`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/Language.vue'))
            })
        },
        meta: {
            permission: 'Pages.Administration.Languages'
        }
    }, { // 语言文本信息
        path: 'languagestext/:lang',
        name: namePre + `languagestext`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/LanguageTextInfo.vue'))
            })
        },
        meta: {
            displayName: lang.L('LanguageTexts')
        }
    }, { // 审计日志
        path: 'audit-logs',
        name: namePre + `AuditLogs`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/AuditLogs.vue'))
            })
        }
    }, { // Maintenance
        path: 'maintenance',
        name: namePre + `Maintenance`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/Maintenance.vue'))
            })
        }
    }, { // 设置
        path: 'settings',
        name: namePre + `Settings.Tenant`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/Settting.vue'))
            })
        }
    }]
}
