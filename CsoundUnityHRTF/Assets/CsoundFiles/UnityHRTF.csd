<CsoundSynthesizer>
<CsOptions>
-odac
</CsOptions>
<CsInstruments> 
sr 	= 	44100 
ksmps 	= 	64
nchnls 	= 	2
0dbfs	=	1 


gasrc init 0
gamix init 0
gadlt init 1
giFile init 100
gasig init 0

instr 1
kenv madsr 0.1, 0, 0.75, 1
a1 buzz kenv*0.25, p4, 5, 1

gasrc = gasrc + a1

endin

instr 107

kAzi chnget "azimuth"
kElev chnget "elevation"


SDataPath chnget "CsoundFiles"

SDatL sprintf "%s/hrtf-44100-left.dat", SDataPath
SDatR sprintf "%s/hrtf-44100-right.dat", SDataPath
gihand fiopen SDatL, 1
gihand2 fiopen SDatR, 1

aleft, aright hrtfmove2 gasrc, kAzi, kElev, SDatL, SDatR

arevl init 0
arevr init 0

arevl = arevl + aleft
arevr = arevr + aright

arev1, arevr, idel hrtfreverb, gasrc, 3, 3, SDatL, SDatR
outs arevl*0.75 + aleft, arevr*0.75 + aright

clear arevl
clear arevr
clear gasrc

endin

</CsInstruments>
<CsScore>
f0 3600
f1 0 4096 10 1
i1 0 z
i107 0 z

</CsScore>
</CsoundSynthesizer>
<bsbPanel>
<label>Widgets</label>
 <objectName/>
 <x>100</x>
 <y>100</y>
 <width>320</width>
<height>240</height>
 <visible>true</visible>
 <uuid/>
 <bgcolor mode="nobackground">
  <r>255</r>
  <g>255</g>
  <b>255</b>
 </bgcolor>
</bsbPanel>
<bsbPresets>
</bsbPresets>
