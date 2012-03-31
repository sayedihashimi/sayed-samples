param($installPath, $toolsPath, $package, $project)
try {
    # Set up variables
    $defaultConnectionString = "Data Source=(LocalDb)\v11.0;Initial Catalog=aspnetdb;Integrated Security=true"
    $oldConnectionName = "ApplicationServices"
    $timestamp = (Get-Date).ToString("yyyyMMddHHmmss")
    $connectionString ="Data Source=(LocalDb)\v11.0;Initial Catalog=aspnet-$($project.ProjectName)-$timestamp;Integrated Security=true"
    $config = $project.ProjectItems | Where-Object { $_.Name -eq "Web.config" }    
    $fileName = $config.Properties | Where-Object { $_.Name -eq "FullPath" }
    
    #Load the Config File
    $xml = New-Object System.Xml.XmlDocument
    $xml.PreserveWhitespace = $true
    $xml.Load($fileName.Value)
    
    # Change the Connection string
    $connectionStrings = $xml.configuration.connectionstrings
    $nodes = $connectionStrings.SelectNodes("add[@name='DefaultConnection']")
    
    if (($nodes.Count -eq 1) -and $nodes.Item(0).connectionString.Equals($defaultConnectionString, 'OrdinalIgnoreCase')) {
        # Only replace the connection string if it exists and looks like something we need to replace.
        $nodes.Item(0).connectionString = $connectionString
        
    }
    elseif ($nodes.Count -gt 1) {
        # If we updated this package, the config transform would've added the $defaultConnectionString to it. We need to remove these.
        $nodes | Where-Object { $_.connectionString.Equals($defaultConnectionString, 'OrdinalIgnoreCase') } | ForEach-Object  {
            $connectionStrings.RemoveChild($_)
        }
    }
    
    function CommentNode($node) {
        if (!$node) {
            return;
        }
        
        $commentNode = $xml.CreateComment($node.OuterXml)
        $parent = $node.ParentNode
        $parent.InsertBefore($commentNode, $node)
        $parent.RemoveChild($node)
    }
    
    $node = $connectionStrings.SelectSingleNode("add[@name='$oldConnectionName'][@providerName='System.Data.SqlClient']")
    CommentNode($node)
     
    # Comment out older providers
    $node = $xml.SelectSingleNode("/configuration/system.web/membership/providers/add[@type='System.Web.Security.SqlMembershipProvider'][@connectionStringName='$oldConnectionName']")
    CommentNode($node)
    
    $node = $xml.SelectSingleNode("/configuration/system.web/profile/providers/add[@type='System.Web.Profile.SqlProfileProvider'][@connectionStringName='$oldConnectionName']")
    CommentNode($node)
    
    $node = $xml.SelectSingleNode("/configuration/system.web/roleManager/providers/add[@type='System.Web.Security.SqlRoleProvider'][@connectionStringName='$oldConnectionName']")
    CommentNode($node)
    
    # Save the Config File 
    $xml.Save($fileName.Value)
    
} catch {
    Write-Output "You will need to manually, change the 'Initial Catalog' in the 'DefaultConnection' to a unique name."
}
# SIG # Begin signature block
# MIIbCQYJKoZIhvcNAQcCoIIa+jCCGvYCAQExCzAJBgUrDgMCGgUAMGkGCisGAQQB
# gjcCAQSgWzBZMDQGCisGAQQBgjcCAR4wJgIDAQAABBAfzDtgWUsITrck0sYpfvNR
# AgEAAgEAAgEAAgEAAgEAMCEwCQYFKw4DAhoFAAQU0vMXqXdC38mwtjqpme37afHI
# aOCgghXyMIIEoDCCA4igAwIBAgIKYRr16gAAAAAAajANBgkqhkiG9w0BAQUFADB5
# MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVk
# bW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSMwIQYDVQQDExpN
# aWNyb3NvZnQgQ29kZSBTaWduaW5nIFBDQTAeFw0xMTExMDEyMjM5MTdaFw0xMzAy
# MDEyMjQ5MTdaMIGDMQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQ
# MA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9u
# MQ0wCwYDVQQLEwRNT1BSMR4wHAYDVQQDExVNaWNyb3NvZnQgQ29ycG9yYXRpb24w
# ggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDDqR/PfCN/MR4GJYnddXm5
# z5NLYZK2lfLvqiWdd/NLWm1JkMzgMbimAjeHdK/yrKBglLjHTiX+h9hY0iBOLfE6
# ZS6SW6Zd5pV14DTlUCGcfTmXto5EI2YWpmUg4Dbrivqd4stgAfwqZMiHRRTxHsrN
# KKy65VdZJtzsxUpsmuYDGikyPwCeg6wlDYTM3W+2arst94Q6bWYx6DZw/4SSkPdA
# dp6ILkfWKxH3j+ASZSu8X+8V/PfsAWi3RQzuwASwDre9eGuujeRQ8TXingHS4etb
# cYJhISDz1MneHLgCRWVJvn61N4anzexa37h2IPwRE1H8+ipQqrQe0DqAvmPK3IFH
# AgMBAAGjggEdMIIBGTATBgNVHSUEDDAKBggrBgEFBQcDAzAdBgNVHQ4EFgQUAAOm
# 5aLEcaKCw492zSwNEuKdSigwDgYDVR0PAQH/BAQDAgeAMB8GA1UdIwQYMBaAFFdF
# dBxdsPbIQwXgjFQtjzKn/kiWMFYGA1UdHwRPME0wS6BJoEeGRWh0dHA6Ly9jcmwu
# bWljcm9zb2Z0LmNvbS9wa2kvY3JsL3Byb2R1Y3RzL01pY0NvZFNpZ1BDQV8wOC0z
# MS0yMDEwLmNybDBaBggrBgEFBQcBAQROMEwwSgYIKwYBBQUHMAKGPmh0dHA6Ly93
# d3cubWljcm9zb2Z0LmNvbS9wa2kvY2VydHMvTWljQ29kU2lnUENBXzA4LTMxLTIw
# MTAuY3J0MA0GCSqGSIb3DQEBBQUAA4IBAQCQ9/h5kmnIj2uKYO58wa4+gThS9LrP
# mYzwLT0T9K72YfB1OE5Zxj8HQ/kHfMdT5JFi1qh2FHWUhlmyuhDCf2wVPxkVww4v
# fjnDz/5UJ1iUNWEHeW1RV7AS4epjcooWZuufOSozBDWLg94KXjG8nx3uNUUNXceX
# 3yrgnX86SfvjSEUy3zZtCW52VVWsNMV5XW4C1cyXifOoaH0U6ml7C1V9AozETTC8
# Yvd7peygkvAOKg6vV5spSM22IaXqHe/cCfWrYtYN7DVfa5nUsfB3Uvl36T9smFbA
# XDahTl4Q9Ix6EZcgIDEIeW5yFl8cMFeby3yiVfVwbHjsoUMgruywNYsYMIIEujCC
# A6KgAwIBAgIKYQUZlgAAAAAAGzANBgkqhkiG9w0BAQUFADB3MQswCQYDVQQGEwJV
# UzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UE
# ChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSEwHwYDVQQDExhNaWNyb3NvZnQgVGlt
# ZS1TdGFtcCBQQ0EwHhcNMTEwNzI1MjA0MjE5WhcNMTIxMDI1MjA0MjE5WjCBszEL
# MAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1v
# bmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjENMAsGA1UECxMETU9Q
# UjEnMCUGA1UECxMebkNpcGhlciBEU0UgRVNOOjlFNzgtODY0Qi0wMzlEMSUwIwYD
# VQQDExxNaWNyb3NvZnQgVGltZS1TdGFtcCBTZXJ2aWNlMIIBIjANBgkqhkiG9w0B
# AQEFAAOCAQ8AMIIBCgKCAQEA08s7U6KfRKN6q01WcVOKd6o3k34BPv2rAqNTqf/R
# sSLFAJDndW7uGOiBDhPF2GEAvh+gdjsEDQTFBKCo/ENTBqEEBLkLkpgCYjjv1DMS
# 9ys9e++tRVeFlSCf12M0nGJGjr6u4NmeOfapVf3P53fmNRPvXOi/SJNPGkMHWDiK
# f4UUbOrJ0Et6gm7L0xVgCBSJlKhbPzrJPyB9bS9YGn3Kiji8w8I5aNgtWBoj7SoQ
# CFogjIKl7dGXRZKFzMM3g98NmHzF07bgmVPYeAj15SMhB2KGWmppGf1w+VM0gfcl
# MRmGh4vAVZr9qkw1Ff1b6ZXJq1OYKV8speElD2TF8rAndQIDAQABo4IBCTCCAQUw
# HQYDVR0OBBYEFHkj56ENvlUsaBgpYoJn1vPhNjhaMB8GA1UdIwQYMBaAFCM0+NlS
# RnAK7UD7dvuzK7DDNbMPMFQGA1UdHwRNMEswSaBHoEWGQ2h0dHA6Ly9jcmwubWlj
# cm9zb2Z0LmNvbS9wa2kvY3JsL3Byb2R1Y3RzL01pY3Jvc29mdFRpbWVTdGFtcFBD
# QS5jcmwwWAYIKwYBBQUHAQEETDBKMEgGCCsGAQUFBzAChjxodHRwOi8vd3d3Lm1p
# Y3Jvc29mdC5jb20vcGtpL2NlcnRzL01pY3Jvc29mdFRpbWVTdGFtcFBDQS5jcnQw
# EwYDVR0lBAwwCgYIKwYBBQUHAwgwDQYJKoZIhvcNAQEFBQADggEBAEfCdoFbMd1v
# 0zyZ8npsfpcTUCwFFxsQuEShtYz0Vs+9sCG0ZG1hHNju6Ov1ku5DohhEw/r67622
# XH+XbUu1Q/snYXgIVHyx+a+YCrR0xKroLVDEff59TqGZ1icot67Y37GPgyKOzvN5
# /GEUbb/rzISw36O7WwW36lT1Yh1sJ6ZjS/rjofq734WWZWlTsLZxmGQmZr3F8Vxi
# vJH0PZxLQgANzzgFFCZa3CoFS39qmTjY3XOZos6MUCSepOv1P4p4zFSZXSVmpEEG
# KK9JxLRSlOzeAoNk/k3U/0ui/CmA2+4/qzztM4jKvyJg0Fw7BLAKtJhtPKc6T5rR
# ARYRYopBdqAwggYHMIID76ADAgECAgphFmg0AAAAAAAcMA0GCSqGSIb3DQEBBQUA
# MF8xEzARBgoJkiaJk/IsZAEZFgNjb20xGTAXBgoJkiaJk/IsZAEZFgltaWNyb3Nv
# ZnQxLTArBgNVBAMTJE1pY3Jvc29mdCBSb290IENlcnRpZmljYXRlIEF1dGhvcml0
# eTAeFw0wNzA0MDMxMjUzMDlaFw0yMTA0MDMxMzAzMDlaMHcxCzAJBgNVBAYTAlVT
# MRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQK
# ExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xITAfBgNVBAMTGE1pY3Jvc29mdCBUaW1l
# LVN0YW1wIFBDQTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAJ+hbLHf
# 20iSKnxrLhnhveLjxZlRI1Ctzt0YTiQP7tGn0UytdDAgEesH1VSVFUmUG0KSrphc
# MCbaAGvoe73siQcP9w4EmPCJzB/LMySHnfL0Zxws/HvniB3q506jocEjU8qN+kXP
# CdBer9CwQgSi+aZsk2fXKNxGU7CG0OUoRi4nrIZPVVIM5AMs+2qQkDBuh/NZMJ36
# ftaXs+ghl3740hPzCLdTbVK0RZCfSABKR2YRJylmqJfk0waBSqL5hKcRRxQJgp+E
# 7VV4/gGaHVAIhQAQMEbtt94jRrvELVSfrx54QTF3zJvfO4OToWECtR0Nsfz3m7IB
# ziJLVP/5BcPCIAsCAwEAAaOCAaswggGnMA8GA1UdEwEB/wQFMAMBAf8wHQYDVR0O
# BBYEFCM0+NlSRnAK7UD7dvuzK7DDNbMPMAsGA1UdDwQEAwIBhjAQBgkrBgEEAYI3
# FQEEAwIBADCBmAYDVR0jBIGQMIGNgBQOrIJgQFYnl+UlE/wq4QpTlVnkpKFjpGEw
# XzETMBEGCgmSJomT8ixkARkWA2NvbTEZMBcGCgmSJomT8ixkARkWCW1pY3Jvc29m
# dDEtMCsGA1UEAxMkTWljcm9zb2Z0IFJvb3QgQ2VydGlmaWNhdGUgQXV0aG9yaXR5
# ghB5rRahSqClrUxzWPQHEy5lMFAGA1UdHwRJMEcwRaBDoEGGP2h0dHA6Ly9jcmwu
# bWljcm9zb2Z0LmNvbS9wa2kvY3JsL3Byb2R1Y3RzL21pY3Jvc29mdHJvb3RjZXJ0
# LmNybDBUBggrBgEFBQcBAQRIMEYwRAYIKwYBBQUHMAKGOGh0dHA6Ly93d3cubWlj
# cm9zb2Z0LmNvbS9wa2kvY2VydHMvTWljcm9zb2Z0Um9vdENlcnQuY3J0MBMGA1Ud
# JQQMMAoGCCsGAQUFBwMIMA0GCSqGSIb3DQEBBQUAA4ICAQAQl4rDXANENt3ptK13
# 2855UU0BsS50cVttDBOrzr57j7gu1BKijG1iuFcCy04gE1CZ3XpA4le7r1iaHOEd
# AYasu3jyi9DsOwHu4r6PCgXIjUji8FMV3U+rkuTnjWrVgMHmlPIGL4UD6ZEqJCJw
# +/b85HiZLg33B+JwvBhOnY5rCnKVuKE5nGctxVEO6mJcPxaYiyA/4gcaMvnMMUp2
# MT0rcgvI6nA9/4UKE9/CCmGO8Ne4F+tOi3/FNSteo7/rvH0LQnvUU3Ih7jDKu3hl
# XFsBFwoUDtLaFJj1PLlmWLMtL+f5hYbMUVbonXCUbKw5TNT2eb+qGHpiKe+imyk0
# BncaYsk9Hm0fgvALxyy7z0Oz5fnsfbXjpKh0NbhOxXEjEiZ2CzxSjHFaRkMUvLOz
# sE1nyJ9C/4B5IYCeFTBm6EISXhrIniIh0EPpK+m79EjMLNTYMoBMJipIJF9a6lbv
# pt6Znco6b72BJ3QGEe52Ib+bgsEnVLaxaj2JoXZhtG6hE6a/qkfwEm/9ijJssv7f
# UciMI8lmvZ0dhxJkAj0tr1mPuOQh5bWwymO0eFQF1EEuUKyUsKV4q7OglnUa2ZKH
# E3UiLzKoCG6gW4wlv6DvhMoh1useT8ma7kng9wFlb4kLfchpyOZu6qeXzjEp/w7F
# W1zYTRuh2Povnj8uVRZryROj/TCCBoEwggRpoAMCAQICCmEVCCcAAAAAAAwwDQYJ
# KoZIhvcNAQEFBQAwXzETMBEGCgmSJomT8ixkARkWA2NvbTEZMBcGCgmSJomT8ixk
# ARkWCW1pY3Jvc29mdDEtMCsGA1UEAxMkTWljcm9zb2Z0IFJvb3QgQ2VydGlmaWNh
# dGUgQXV0aG9yaXR5MB4XDTA2MDEyNTIzMjIzMloXDTE3MDEyNTIzMzIzMloweTEL
# MAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1v
# bmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEjMCEGA1UEAxMaTWlj
# cm9zb2Z0IENvZGUgU2lnbmluZyBQQ0EwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAw
# ggEKAoIBAQCfjd+FN4yxBlZmNk7UCus2I5Eer6uNWOnEz8GfOgokxMTEXrDuFRTF
# +j6ZM2sZaXL0fAVf5ZklRNc1GYqQ3CiOkAzv1ZBhrd7cGHAtg8lvr4Us+N25uTD9
# cXgcg/3IqbmCZw16uMEJwrwWl1c/HJjTadcwkJCQjTAf2CbUnnuI2eIJ7ZdJResE
# UoF1e7i1IrguVrvXz6lOPAqDoqg6xa22AQ5qzyK0Ix9s1Sfnt37BtNUyrXklHEKG
# 4p2F9FfaG1kvLSaSKcWz14WjnmBalOZ7nHtegjRLbf/U7ifQotzRkAzOfQ4VfIis
# NMfAbJiESslEeWgo3yKDDbiKLEhh4v4RAgMBAAGjggIjMIICHzAQBgkrBgEEAYI3
# FQEEAwIBADAdBgNVHQ4EFgQUV0V0HF2w9shDBeCMVC2PMqf+SJYwCwYDVR0PBAQD
# AgHGMA8GA1UdEwEB/wQFMAMBAf8wgZgGA1UdIwSBkDCBjYAUDqyCYEBWJ5flJRP8
# KuEKU5VZ5KShY6RhMF8xEzARBgoJkiaJk/IsZAEZFgNjb20xGTAXBgoJkiaJk/Is
# ZAEZFgltaWNyb3NvZnQxLTArBgNVBAMTJE1pY3Jvc29mdCBSb290IENlcnRpZmlj
# YXRlIEF1dGhvcml0eYIQea0WoUqgpa1Mc1j0BxMuZTBQBgNVHR8ESTBHMEWgQ6BB
# hj9odHRwOi8vY3JsLm1pY3Jvc29mdC5jb20vcGtpL2NybC9wcm9kdWN0cy9taWNy
# b3NvZnRyb290Y2VydC5jcmwwVAYIKwYBBQUHAQEESDBGMEQGCCsGAQUFBzAChjho
# dHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtpL2NlcnRzL01pY3Jvc29mdFJvb3RD
# ZXJ0LmNydDB2BgNVHSAEbzBtMGsGCSsGAQQBgjcVLzBeMFwGCCsGAQUFBwICMFAe
# TgBDAG8AcAB5AHIAaQBnAGgAdAAgAKkAIAAyADAAMAA2ACAATQBpAGMAcgBvAHMA
# bwBmAHQAIABDAG8AcgBwAG8AcgBhAHQAaQBvAG4ALjATBgNVHSUEDDAKBggrBgEF
# BQcDAzANBgkqhkiG9w0BAQUFAAOCAgEAMLywIKRioKfvOSZhPdysxpnQhsQu9YMy
# ZV4iPpvWhvjotp/Ki9Y7dQuhkT5M3WR0jEnyiIwYZ2z+FWZGuDpGQpfIkTfUJLHn
# rNPqQRSDd9PJTwVfoxRSv5akLz5WWxB1zlPDzgVUabRlySSlD+EluBq5TeUCuVAe
# T7OYDB2VAu4iWa0iywV0CwRFewRZ4NgPs+tM+GDdwnie0bqfa/fz7n5EEUDSvbqb
# SxYIbqS+VeSmOBKjSPQcVXqKINF9/pHblI8vwntrpmSFT6PlLDQpXQu/9cc4L8Qg
# xFYx9mnOhfgKkezQ1q66OAUM625PTJwDKaqi/BigKQwNXFxWI1faHJYNyCY2wUTL
# 5eHmb4nnj+mYtXPTeOPtowE8dOVevGz2IYlnBeyXnbWx/a+m6XKlwzThL5/59Go5
# 4i0Eglv80JyufJ0R+ea1Uxl0ujlKOet9QrNKOzc9wkp7J5jn4k6bG0pUOGojN75q
# t0ju6kINSSSRjrcELpdv5OdFu49N/WDZ11nC2IDWYDR7t6GTIP6BuKqlXAnpig2+
# KE1+1+gP7WV40TFfuWbb30LnC8wCB43f/yAGo0VltLMyjS6R4k20qcn6vGsEDrKf
# 6p/epMkKlvSN99iYqPCFAghZpCCmLAsa8lIG7WnlZBgb4KOr3sp8FGFDuGX1NqNV
# EytnLE0bMEwxggSBMIIEfQIBATCBhzB5MQswCQYDVQQGEwJVUzETMBEGA1UECBMK
# V2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0
# IENvcnBvcmF0aW9uMSMwIQYDVQQDExpNaWNyb3NvZnQgQ29kZSBTaWduaW5nIFBD
# QQIKYRr16gAAAAAAajAJBgUrDgMCGgUAoIGuMBkGCSqGSIb3DQEJAzEMBgorBgEE
# AYI3AgEEMBwGCisGAQQBgjcCAQsxDjAMBgorBgEEAYI3AgEVMCMGCSqGSIb3DQEJ
# BDEWBBS/fINScjzS+8MGpulhPPmFClkwbDBOBgorBgEEAYI3AgEMMUAwPqAkgCIA
# TQBpAGMAcgBvAHMAbwBmAHQAIABBAFMAUAAuAE4ARQBUoRaAFGh0dHA6Ly93d3cu
# YXNwLm5ldC8gMA0GCSqGSIb3DQEBAQUABIIBAIFiFYVOoktqDrhR5ZkpD2xfdrsz
# rUdyK10a5oTMK9F00we/mw+tN9COesdsG+wqmO6qOQiwsMNFHUm67vr4l2DSn+KQ
# lQsuTST0aBTIOvcb71ub1HTcReP1jhz8UpmPQIzV1DlrktK2JiCW6H39AblFpnO9
# Myc1q3Wvav9txiAiZq1YB01lwn4J8qGy88l+ty2IrcXLzeeBpRBe3fiJRFLPX84K
# oLE2dPcdjNzykq6ElC+EW3UGb4ScHevE0DDw64eBpP0xmeRcHAFSu2oV8WavYNkq
# 13tT2h9SLSo12PhpoPyHQaOsxZk3PgfGCnbnfjUoRnaqtTJM6C96gC9PzmyhggId
# MIICGQYJKoZIhvcNAQkGMYICCjCCAgYCAQEwgYUwdzELMAkGA1UEBhMCVVMxEzAR
# BgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1p
# Y3Jvc29mdCBDb3Jwb3JhdGlvbjEhMB8GA1UEAxMYTWljcm9zb2Z0IFRpbWUtU3Rh
# bXAgUENBAgphBRmWAAAAAAAbMAcGBSsOAwIaoF0wGAYJKoZIhvcNAQkDMQsGCSqG
# SIb3DQEHATAcBgkqhkiG9w0BCQUxDxcNMTIwMjIzMTkyOTE1WjAjBgkqhkiG9w0B
# CQQxFgQUZPrsJdAI7JMja2WZEWuSoW7S4jQwDQYJKoZIhvcNAQEFBQAEggEAphkk
# aR/a0XuLS2w/H8ithOvMXCtcjLkwKOmgdpqcEs/ZLq8vwlX1zX5JkEbF8E7QCjV5
# j3cGEpyBD5YInfly6Xbi4YnhUimGkfl/ABXaMdA4CAoOwYap5iZenoHOElgUwyst
# YjWylj6ckB1pHlToOTc4lgN3758iUp/4C4fYqgG/c6Vhw2leJrMIqzIDtvXn6BYz
# 8HRglX30rJr3xCzD4OnUNYhzAKE0+JSLIRBbbjTc7gRpJsfUEsOjj5hhSwSN0nHi
# bsrPGMqBh1ayrwV4QuBxlDpVF56sBm7U/Lr/RKEnVIfR2PjY6/ylDAypObEKLv/0
# 91fcicpSeXq2dqcr/w==
# SIG # End signature block
