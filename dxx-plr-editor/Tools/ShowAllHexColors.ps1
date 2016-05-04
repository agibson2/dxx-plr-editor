# Script to automate changing the macros to display all the colors possible with macros
# to determine the best colors to use for the macro / escape colors /f9 "/wThis Is White", etc
# It fills all the macros f9 through f12 with colors and launches d2 so you can host
# a network game and then press f9 through f12 to see as many colors as fit in f9 through f12.
# When you quit descent 2 it will then fill the f9 through f12 again with more colors to look at
# and keep doing that until all 01 through FE (254) hex numbers have been shown.
# It works with descent 1 and descent 2

# Change to your location of dxx-plr-editor.exe
$dxxplreditor = "Z:\games\descent2\dxx-plr-editor.exe"
# Descent 2 exe path and filename
$descent2 = "Z:\games\descent\d1x-rebirth-retro-1.3.exe"
# Change to your plr file location and filename
$plrfile = "Z:\games\descent\static.plr"

echo "WARNING: this script overwrites your .plr file. Make sure to back it up just in case."
echo "         It also requires editing the script to point to the dxx-plr-editor and the"
echo "         location of your plr file."
read-host "Press ENTER to start or CRTL-C to abort"

$overwrite = "y"

$count = 0
while ( $count -lt 254 ) {
    #echo $count
    $optionstring = ""
    $fcount = 9
    $arguments = @()
    $arguments += ,$plrfile
    if ($overwrite -match "y") {
        $arguments += ,"/overwrite"
    }
    while($fcount -lt 13) {
        $fstring = ""
        $hexcount = 0
        while($hexcount -lt 8 -and $count -lt 254) {
            $count++
            $fstring = $fstring + "/x{0:X2}{0:X2}" -f $count
            $hexcount++
        }
        $arguments += ,"/f$fcount"
        $arguments += ,$fstring
        $optionstring = "$optionstring /f$fcount $fstring"
        #echo "$optionstring"
        $fcount++
    }

    $hexcount++
    # we use | write-host to make sure we wait until the exe is done before proceding
    # otherwise the script will continue before descent 2 exits.
    & $dxxplreditor $arguments | write-host
    #echo $argument
    sleep 2
    & $descent2 | write-host
}