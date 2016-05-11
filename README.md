# UnityVOIP
VOIP for Unity.

NOT PRODUCTION TESTED, MIGHT NOT WORK.
This is currently being developed, I am open sourcing it due to the lack of decent VOIP options for Unity.

Requires the NSpeex.dll
https://nspeex.codeplex.com/

This does not provide any networking, you will need to plug it into whatever you use. Photon, UNET etc.

VoipListener is used to listen for data from the microphone, you only need one of these per client.
VoipSpeaker is used to play audio, you would typically attach this to the gameobjects used to represent other players.
You will want one VoipSpeaker per remote VoipListener. i.e. if you are playing against 10 people you want 9 VoipSpeakers and 1 VoipListener.

How to use:

Add the VoipListener component and listen to the OnAudioGenerated event.
Send the VoipFragment from the event to the server/client.
When you recieve a VoipFragment call Recieve on the VoipSpeaker that should play this audio.
You will need to match the VoipFragments to the correct VoipSpeaker, e.g. you could send a unique client id along with every VoipFragment so it could be matched against the correct VoipSpeaker.